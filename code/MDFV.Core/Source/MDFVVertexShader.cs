using Silk.NET.OpenGL;

namespace MDFV.Core.Source;

public class MDFVVertexShader
    : MDFVShader
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public MDFVVertexShader(GL gl, string source)
        : base(gl, GLEnum.VertexShader, source)
    {
        
    }
}