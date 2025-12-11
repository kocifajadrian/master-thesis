using Silk.NET.OpenGL;

namespace MDFV.Core.Source;

public class MDFVIndexBuffer
{
    private readonly GL _gl;
    private readonly uint _id;

    public MDFVIndexBuffer(GL gl, uint[] indices)
    {
        _gl = gl;
        _id = _gl.GenBuffer();
        Bind();
        _gl.BufferData(
            GLEnum.ElementArrayBuffer,
            (ReadOnlySpan<uint>)indices.AsSpan(),
            GLEnum.StaticDraw);
    }

    public void Bind()
    {
        _gl.BindBuffer(GLEnum.ElementArrayBuffer, _id);
    }

    public void Unbind()
    {
        _gl.BindBuffer(GLEnum.ElementArrayBuffer, 0);
    }
    
    public void Delete()
    {
        _gl.DeleteBuffer(_id);
    }
}