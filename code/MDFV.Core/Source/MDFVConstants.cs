using System.Numerics;

namespace MDFV.Core.Source;

public static class MDFVConstants
{
    public const string WindowTitle = "MultiDimensionalFunctionVisualization";
    public const int DefaultScreenResolutionWidth = 1800;
    public const int DefaultScreenResolutionHeight = 900;
    public static readonly Vector3 DefaultBoundingBoxStart = new(-5.0f, -5.0f, -5.0f);
    public const float DefaultBoundingBoxSize = 10.0f;
    public const int DefaultVoxelResolution = 512;
    public const uint DefaultSlicesCount = 1;
    public const float DefaultSlice = 0.0f;
    public static readonly Vector3 DefaultSliceColor = new(1.0f, 1.0f, 1.0f);
    public static readonly float[] DefaultSlices = [DefaultSlice];
    public static readonly Vector3[] DefaultSlicesColors = [DefaultSliceColor];
    
    public static readonly Vector3 DefaultCameraPosition = new(0.0f, 0.0f, 3.0f);
    public static readonly Vector3 DefaultCameraForward = new(0.0f, 0.0f, -1.0f);
    public static readonly Vector3 DefaultCameraRight = new(1.0f, 0.0f, 0.0f);
    public static readonly Vector3 DefaultCameraUp = new(0.0f, 1.0f, 0.0f);
    public static readonly Vector3 DefaultCameraWorldUp = new(0.0f, 1.0f, 0.0f);
    public const float DefaultCameraFov = 60.0f;
    public const float DefaultCameraFovMultiplier = 0.95f;
    public const float DefaultCameraYaw = -90.0f;
    public const float DefaultCameraPitch = 0.0f;
    public const float MinimumCameraPitch = -89.0f;
    public const float MaximumCameraPitch = 89.0f;
    public const float DefaultCameraSpeed = 1.0f;
    public const float DefaultCameraSensitivity = 100.0f;
    public const float DefaultCameraZoom = 0.05f;
    public const bool DefaultMouseClick = false;
    public static readonly Vector2 DefaultMouseLastPosition = new(-1.0f, -1.0f);
    
    public const int ShaderCompileErrorStatus = 0;
    public const int ShaderProgramLinkErrorStatus = 0;
    
    public static readonly float[] Vertices =
    [
        -1f, -1f, 0f,
        1f, -1f, 0f,
        1f,  1f, 0f,
        -1f,  1f, 0f
    ];
    
    public const int IndicesCount = 6;
    public static readonly uint[] Indices =
    [
        0, 1, 2,
        2, 3, 0
    ];
}