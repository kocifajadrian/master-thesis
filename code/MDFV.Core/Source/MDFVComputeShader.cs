using Silk.NET.OpenGL;

namespace MDFV.Core.Source;

public class MDFVComputeShader
    : MDFVShader
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public MDFVComputeShader(GL gl, string source)
        : base(gl, GLEnum.ComputeShader, source)
    {
        
    }
}