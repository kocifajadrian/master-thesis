using Silk.NET.OpenGL;

namespace MDFV.Core.Source;

public class MDFVVertexArray
{
    private readonly GL _gl;
    private readonly uint _id;

    public MDFVVertexArray(GL gl)
    {
        _gl = gl;
        _id = _gl.GenVertexArray();
    }

    public void Bind()
    {
        _gl.BindVertexArray(_id);
    }

    public void Unbind()
    {
        _gl.BindVertexArray(0);
    }

    public void Delete()
    {
        _gl.DeleteVertexArray(_id);
    }
}