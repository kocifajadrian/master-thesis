using Silk.NET.OpenGL;

namespace MDFV.Core.Source;

public class MDFVShaderProgram
{
    private readonly GL _gl;
    private readonly uint _id;

    // ReSharper disable once ConvertToPrimaryConstructor
    public MDFVShaderProgram(GL gl)
    {
        _gl = gl;
        _id = gl.CreateProgram();
    }
    
    public uint GetId() => _id;
    
    public void Attach(MDFVShader shader)
    {
        _gl.AttachShader(_id, shader.GetId());
    }

    public void Link()
    {
        _gl.LinkProgram(_id);
        _gl.GetProgram(_id, GLEnum.LinkStatus, out var status);
        
        switch (status)
        {
            case MDFVConstants.ShaderProgramLinkErrorStatus:
            {
                var errorMessage = _gl.GetProgramInfoLog(_id);
                throw new Exception($"Error linking shader program. Error: `{errorMessage}`.");
            }
        }
    }

    public void Use()
    {
        _gl.UseProgram(_id);
    }

    public void Delete()
    {
        _gl.DeleteProgram(_id);
    }
}