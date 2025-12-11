using Silk.NET.OpenGL;

namespace MDFV.Core.Source;

public abstract class MDFVUniforms
{
    protected readonly GL Gl;
    private readonly uint _programId;
    
    // ReSharper disable once ConvertToPrimaryConstructor
    protected MDFVUniforms(GL gl, uint programId)
    {
        Gl = gl;
        _programId = programId;
    }

    protected int GetLocation(string property)
    {
        return Gl.GetUniformLocation(_programId, property);
    }
}