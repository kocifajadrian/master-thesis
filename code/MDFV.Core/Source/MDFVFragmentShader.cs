using Silk.NET.OpenGL;

namespace MDFV.Core.Source;

public class MDFVFragmentShader
    : MDFVShader
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public MDFVFragmentShader(GL gl, string source)
        : base(gl, GLEnum.FragmentShader, source)
    {
        
    }
}