using Silk.NET.OpenGL;

namespace MDFV.Core.Source;

public abstract class MDFVShader
{
    private readonly GL _gl;
    private readonly uint _id;

    protected MDFVShader(GL gl, GLEnum shaderType, string source)
    {
        _gl = gl;
        _id = _gl.CreateShader(shaderType);
        _gl.ShaderSource(_id, source);
        _gl.CompileShader(_id);
        _gl.GetShader(_id, GLEnum.CompileStatus, out var status);
        
        switch (status)
        {
            case MDFVConstants.ShaderCompileErrorStatus:
            {
                var errorMessage = _gl.GetShaderInfoLog(_id);
                throw new Exception($"Error compiling shader. Error: `{errorMessage}`.");
            }
        }
    }

    public uint GetId()
    {
        return _id;
    }

    public void Delete()
    {
        _gl.DeleteShader(_id);
    }
}