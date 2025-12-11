#version 460 core
#define STORAGE_HELPER 32
#define MAX_SLICES 64

out vec4 out_color;

struct ScreenResolution {
    int width;
    int height;
};

struct Camera {
    vec3 position;
    vec3 forward;
    vec3 right;
    vec3 up;
    float fov;
};

struct Box {
    vec3 start;
    vec3 end;
};

struct Color {
    vec4 white;
    vec4 black;
    vec4 background;
};

uniform ScreenResolution screenResolution;
uniform int voxelResolution;
uniform Camera camera;
uniform usampler3D voxelTexture;
uniform uint slicesCount;
uniform vec3 slicesColors[MAX_SLICES];

const Box box = Box(
vec3(-1.0, -1.0, -1.0),
vec3(1.0, 1.0, 1.0)
);

const Color color = Color(
vec4(1.0, 1.0, 1.0, 1.0),
vec4(0.0, 0.0, 0.0, 1.0),
vec4(0.1, 0.1, 0.1, 1.0)
);

vec2 adjustCoordinates(vec2 coordinates) {
    vec2 result = (coordinates / vec2(screenResolution.width, screenResolution.height)) * 2.0 - 1.0;
    result.x *= screenResolution.width / screenResolution.height;
    return result;
}

vec3 getRayDirection(vec2 position) {
    float value = tan(radians(camera.fov) / 2.0);
    vec3 direction = camera.forward +
    position.x * value * camera.right +
    position.y * value * camera.up;
    return normalize(direction);
}

bool inBox(vec3 position) {
    return position.x >= box.start.x && position.x <= box.end.x &&
    position.y >= box.start.y && position.y <= box.end.y &&
    position.z >= box.start.z && position.z <= box.end.z;
}

vec2 boxIntersection(vec3 position, vec3 direction) {
    vec3 inverseDirection = 1.0 / direction;
    float epsilon = 1e-6;

    vec3 v1 = (box.start - position + epsilon) * inverseDirection;
    vec3 v2 = (box.end - position - epsilon) * inverseDirection;

    vec3 v_minimum = min(v1, v2);
    vec3 v_maximum = max(v1, v2);

    float near = max(max(v_minimum.x, v_minimum.y), v_minimum.z);
    float far  = min(min(v_maximum.x, v_maximum.y), v_maximum.z);

    if (near < 0.0 && inBox(position)) near = 0.0;
    if (near > far || far < 0.0) return vec2(-1.0, -1.0);

    return vec2(near, far);
}

ivec3 worldToVoxelCoordinates(vec3 position) {
    return ivec3(floor((position - box.start) /
    (box.end - box.start) *
    float(voxelResolution)));
}

void initializeVoxelTraversal(
vec3 rayDirection,
vec3 currentPosition,
out vec3 step,
out vec3 maximum,
out vec3 delta
) {
    for (int i = 0; i < 3; ++i) {
        if (rayDirection[i] < 0.0) {
            step[i] = -1.0;
            maximum[i] = ((floor(currentPosition[i]) - currentPosition[i]) / rayDirection[i]);
            delta[i] = -1.0 / rayDirection[i];
        } else {
            step[i] = 1.0;
            maximum[i] = ((ceil(currentPosition[i]) - currentPosition[i]) / rayDirection[i]);
            delta[i] = 1.0 / rayDirection[i];
        }
    }
}

const float voxelOpacity = 0.05;

void main() {

    vec2 position = adjustCoordinates(gl_FragCoord.xy);
    vec3 rayDirection = getRayDirection(position);
    vec2 intersections = boxIntersection(camera.position, rayDirection);

    if (intersections.x < 0.0) {
        out_color = color.black;
        return;
    }

    vec3 rayOrigin = camera.position + (intersections.x + 1e-4) * rayDirection;
    float voxelWorldSize = length((box.end - box.start) / float(voxelResolution));

    vec3 gridPosition = (rayOrigin - box.start) /
    (box.end - box.start) *
    float(voxelResolution);

    vec3 step, maximum, delta;
    initializeVoxelTraversal(rayDirection, gridPosition, step, maximum, delta);

    ivec3 voxelCoordinates = worldToVoxelCoordinates(rayOrigin);
    vec4 accumColor = vec4(0.0);

    vec3 C_accum = vec3(0.0);
    float A_accum = 0.0;

    for (int i = 0; i < voxelResolution * 3; ++i) {

        bool outside =
        voxelCoordinates.x < 0 || voxelCoordinates.x >= voxelResolution ||
        voxelCoordinates.y < 0 || voxelCoordinates.y >= voxelResolution ||
        voxelCoordinates.z < 0 || voxelCoordinates.z >= voxelResolution;

        if (outside) break;

        vec3 voxelStart = box.start +
        vec3(voxelCoordinates) *
        (box.end - box.start) / float(voxelResolution);

        vec3 voxelEnd = voxelStart +
        (box.end - box.start) / float(voxelResolution);

        vec3 invDir = 1.0 / rayDirection;

        vec3 t1 = (voxelStart - rayOrigin) * invDir;
        vec3 t2 = (voxelEnd - rayOrigin) * invDir;

        vec3 tmin = min(t1, t2);
        vec3 tmax = max(t1, t2);

        float tEnter = max(max(tmin.x, tmin.y), tmin.z);
        float tExit  = min(min(tmax.x, tmax.y), tmax.z);

        float distanceInVoxel = max(0.0, tExit - tEnter);

        vec3 normalizedCoords = (vec3(voxelCoordinates) + 0.5) / float(voxelResolution);
        uint voxel = texture(voxelTexture, normalizedCoords).r;

        // Compute the voxel index along X
        int voxelX = voxelCoordinates.x;

        // Compute which uint and which bit
        int uintX = voxelX / STORAGE_HELPER;
        int bitIndex = voxelX % STORAGE_HELPER;

        // Adjust normalized coordinates for texture lookup
        vec3 texCoords = vec3(
        (float(uintX) + 0.5) / float((voxelResolution + (STORAGE_HELPER - 1)) / STORAGE_HELPER),
        (float(voxelCoordinates.y) + 0.5) / float(voxelResolution),
        (float(voxelCoordinates.z) + 0.5) / float(voxelResolution)
        );

        for (int s = 0; s < slicesCount; ++s) {
            // depth offset: each slice is stacked in Z
            int depthIndex = voxelCoordinates.z + s * voxelResolution;

            vec3 texCoords = vec3(
            (float(uintX) + 0.5) / float((voxelResolution + (STORAGE_HELPER - 1)) / STORAGE_HELPER),
            (float(voxelCoordinates.y) + 0.5) / float(voxelResolution),
            (float(depthIndex) + 0.5) / float(voxelResolution * slicesCount)
            );

            uint voxelInt = texture(voxelTexture, texCoords).r;
            uint voxelBit = (voxelInt >> bitIndex) & 1u;

            if (voxelBit == 1u && distanceInVoxel > 0.0) {

                // Assign color per slice
                vec3 voxelColor = slicesColors[s];
                float alpha = voxelOpacity * distanceInVoxel / voxelWorldSize;

                accumColor.rgb += (1.0 - accumColor.a) * alpha * voxelColor;
                accumColor.a   += (1.0 - accumColor.a) * alpha;

                if (accumColor.a >= 0.99) break;
            }
        }


        if (maximum.x < maximum.y && maximum.x < maximum.z) {
            voxelCoordinates.x += int(step.x);
            maximum.x += delta.x;
        } else if (maximum.y < maximum.z) {
            voxelCoordinates.y += int(step.y);
            maximum.y += delta.y;
        } else {
            voxelCoordinates.z += int(step.z);
            maximum.z += delta.z;
        }
    }

    if (accumColor.a < 0.01)
    accumColor = color.black;

    out_color = accumColor;
}
