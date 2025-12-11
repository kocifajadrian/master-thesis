using Silk.NET.OpenGL;

namespace MDFV.Core.Source;

public class MDFVComputeUniforms : MDFVUniforms
{
    private readonly int _boundingBoxStart;
    private readonly int _boundingBoxSize;
    private readonly int _voxelResolution;
    private readonly int _slicesCountLocation;
    private readonly int _slicesLocation;
    
    public MDFVComputeUniforms(GL gl, uint programId) : base(gl, programId)
    {
        _boundingBoxStart = GetLocation("boundingBox.start");
        _boundingBoxSize = GetLocation("boundingBox.size");
        _voxelResolution = GetLocation("voxelResolution");
        _slicesCountLocation = GetLocation("slicesCount");
        _slicesLocation = GetLocation("slices");
    }

    public void SetUniforms(MDFVSettings settings)
    {
        SetBoundingBoxUniforms(settings);
        SetVoxelResolutionUniforms(settings);
        SetSlicesUniforms(settings);
    }
    
    private void SetBoundingBoxUniforms(MDFVSettings settings)
    {
        Gl.Uniform3(_boundingBoxStart, settings.CurrentBoundingBox.Start);
        Gl.Uniform1(_boundingBoxSize, settings.CurrentBoundingBox.Size);
    }
    
    private void SetVoxelResolutionUniforms(MDFVSettings settings)
    {
        Gl.Uniform1(_voxelResolution, settings.CurrentVoxelResolution);
    }
    
    private void SetSlicesUniforms(MDFVSettings settings)
    {
        Gl.Uniform1(_slicesCountLocation, settings.CurrentSlicesCount);
        Gl.Uniform1(_slicesLocation, settings.CurrentSlicesCount, settings.CurrentSlices);
    }
}