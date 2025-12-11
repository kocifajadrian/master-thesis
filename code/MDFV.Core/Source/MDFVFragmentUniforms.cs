using Silk.NET.OpenGL;

namespace MDFV.Core.Source;

public class MDFVFragmentUniforms : MDFVUniforms
{
    private readonly int _screenResolutionWidthLocation;
    private readonly int _screenResolutionHeightLocation;
    private readonly int _voxelResolution;
    private readonly int _cameraPositionLocation;
    private readonly int _cameraForwardLocation;
    private readonly int _cameraRightLocation;
    private readonly int _cameraUpLocation;
    private readonly int _cameraFovLocation;
    private readonly int _slicesCountLocation;
    private readonly int _slicesColorsLocation;
    
    public MDFVFragmentUniforms(GL gl, uint programId) : base(gl, programId)
    {
        _screenResolutionWidthLocation = GetLocation("screenResolution.width");
        _screenResolutionHeightLocation = GetLocation("screenResolution.height");
        _voxelResolution = GetLocation("voxelResolution");
        _cameraPositionLocation = GetLocation("camera.position");
        _cameraForwardLocation = GetLocation("camera.forward");
        _cameraRightLocation = GetLocation("camera.right");
        _cameraUpLocation = GetLocation("camera.up");
        _cameraFovLocation = GetLocation("camera.fov");
        _slicesCountLocation = GetLocation("slicesCount");
        _slicesColorsLocation = GetLocation("slicesColors");
    }

    public void SetUniforms(MDFVSettings settings, MDFVCamera camera)
    {
        SetScreenResolutionUniforms(settings);
        SetVoxelResolutionUniforms(settings);
        SetCameraUniforms(camera);
        SetSlicesUniforms(settings);
    }
    
    private void SetScreenResolutionUniforms(MDFVSettings settings)
    {
        Gl.Uniform1(_screenResolutionWidthLocation, settings.ScreenResolution.Width);
        Gl.Uniform1(_screenResolutionHeightLocation, settings.ScreenResolution.Height);
    }
    
    private void SetVoxelResolutionUniforms(MDFVSettings settings)
    {
        Gl.Uniform1(_voxelResolution, settings.CurrentVoxelResolution);
    }
    
    private void SetCameraUniforms(MDFVCamera camera)
    {
        Gl.Uniform3(_cameraPositionLocation, camera.Position);
        Gl.Uniform3(_cameraForwardLocation, camera.Forward);
        Gl.Uniform3(_cameraRightLocation, camera.Right);
        Gl.Uniform3(_cameraUpLocation, camera.Up);
        Gl.Uniform1(_cameraFovLocation, camera.Fov);
    }
    
    private void SetSlicesUniforms(MDFVSettings settings)
    {
        var colors = new float[settings.CurrentSlicesCount * 3];
        for (uint i = 0; i < settings.CurrentSlicesCount; i++)
        {
            colors[i * 3 + 0] = settings.CurrentSlicesColors[i].X;
            colors[i * 3 + 1] = settings.CurrentSlicesColors[i].Y;
            colors[i * 3 + 2] = settings.CurrentSlicesColors[i].Z;
        }
        
        Gl.Uniform1(_slicesCountLocation, settings.CurrentSlicesCount);
        Gl.Uniform3(_slicesColorsLocation, settings.CurrentSlicesCount, colors);
    }
}