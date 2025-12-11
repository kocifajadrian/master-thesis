using Silk.NET.OpenGL;

namespace MDFV.Core.Source;

public class MDFVVertexBuffer
{
    private readonly GL _gl;
    private readonly uint _id;

    public MDFVVertexBuffer(GL gl, float[] vertices)
    {
        _gl = gl;
        _id = _gl.GenBuffer();
        Bind();
        _gl.BufferData(
            GLEnum.ArrayBuffer,
            (ReadOnlySpan<float>)vertices.AsSpan(),
            GLEnum.StaticDraw);
    }

    public void Bind()
    {
        _gl.BindBuffer(GLEnum.ArrayBuffer, _id);
    }

    public void Unbind()
    {
        _gl.BindBuffer(GLEnum.ArrayBuffer, 0);
    }

    public void Delete()
    {
        _gl.DeleteBuffer(_id);
    }
}