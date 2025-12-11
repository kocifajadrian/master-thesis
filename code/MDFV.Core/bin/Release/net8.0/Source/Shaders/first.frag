#version 460 core

out vec4 out_color;

struct Resolution {
    float width;
    float height;
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
    int size;
};

struct Color {
    vec4 white;
    vec4 black;
    vec4 background;
};

uniform Resolution resolution;
uniform Camera camera;
uniform int size;

const Box box = Box(
vec3(-1.0, -1.0, -1.0),
vec3(1.0, 1.0, 1.0),
size
);

const Color color = Color(
vec4(1.0, 1.0, 1.0, 1.0),
vec4(0.0, 0.0, 0.0, 1.0),
vec4(0.1, 0.1, 0.1, 1.0)
);

uniform usampler3D voxelTex;

vec2 adjustCoordinates(vec2 coordinates) {
    vec2 result = (coordinates / vec2(resolution.width, resolution.height)) * 2.0 - 1.0;
    result.x *= resolution.width / resolution.height;
    return result;
}

vec3 getRayDirection(vec2 position) {
    float value = tan(radians(camera.fov) / 2.0);
    vec3 direction =
    camera.forward +
    position.x * value * camera.right +
    position.y * value * camera.up;
    return normalize(direction);
}

bool inBox(vec3 position) {
    return
    position.x >= box.start.x && position.x <= box.end.x &&
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
    float far = min(min(v_maximum.x, v_maximum.y), v_maximum.z);
    if (near < 0.0 && inBox(position)) near = 0.0;
    if (near > far || far < 0.0) return vec2(-1.0, -1.0);
    
    return vec2(near, far);
}

ivec3 worldToVoxelCoordinates(vec3 position)
{
    return ivec3(floor((position - box.start) / (box.end - box.start) * float(box.size)));
}

void initializeVoxelTraversal(vec3 rayDirection, vec3 currentPosition, out vec3 step, out vec3 maximum, out vec3 delta)
{
    for (int i = 0; i < 3; ++i)
    {
        if (rayDirection[i] < 0.0)
        {
            step[i] = -1.0;
            maximum[i] = ((floor(currentPosition[i]) - currentPosition[i]) / rayDirection[i]);
            delta[i] = -1.0 / rayDirection[i];
        }
        else
        {
            step[i] = 1.0;
            maximum[i] = ((ceil(currentPosition[i]) - currentPosition[i]) / rayDirection[i]);
            delta[i] = 1.0 / rayDirection[i];
        }
    }
}

void main()
{
    vec2 position = adjustCoordinates(gl_FragCoord.xy);
    vec3 rayDirection = getRayDirection(position);
    vec2 intersections = boxIntersection(camera.position, rayDirection);

    if (intersections.x < 0.0)
    {
        out_color = color.background;
        return;
    }

    vec3 start = camera.position + intersections.x * rayDirection;
    float distance = intersections.y - intersections.x;
    vec3 gridPosition = (start - box.start) / (box.end - box.start) * float(box.size);
    vec3 step, maximum, delta;
    initializeVoxelTraversal(rayDirection, gridPosition, step, maximum, delta);
    ivec3 voxelCoordinates = worldToVoxelCoordinates(start);

    for (int i = 0; i < box.size * 3; ++i)
    {
        bool outside =
        voxelCoordinates.x < 0 || voxelCoordinates.x >= box.size ||
        voxelCoordinates.y < 0 || voxelCoordinates.y >= box.size ||
        voxelCoordinates.z < 0 || voxelCoordinates.z >= box.size;

        if (outside)
        {
            out_color = color.background;
            return;
        }

        // Normalize voxel coordinates to [0,1] for color
        vec3 normalizedCoords = (vec3(voxelCoordinates) + 0.5) / float(box.size);

        // Option 1: Display voxel coordinates as RGB
        out_color = vec4(normalizedCoords, 1.0);

        // Optional: sample the voxel texture to decide whether to display or not
        uint voxel = texture(voxelTex, normalizedCoords).r;

        if (voxel == 1u)
        {
            // Blend voxel texture presence with coordinate color
            out_color = vec4(normalizedCoords * 0.5 + 0.5, 1.0);
            return;
        }

        if (maximum.x < maximum.y && maximum.x < maximum.z)
        {
            voxelCoordinates.x += int(step.x);
            maximum.x += delta.x;
        }
        else if (maximum.y < maximum.z)
        {
            voxelCoordinates.y += int(step.y);
            maximum.y += delta.y;
        }
        else
        {
            voxelCoordinates.z += int(step.z);
            maximum.z += delta.z;
        }
    }

    out_color = color.background;
}