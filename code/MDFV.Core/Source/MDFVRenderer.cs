using Silk.NET.OpenGL;

namespace MDFV.Core.Source;

public class MDFVRenderer
{
    private readonly MDFVContext _context;
    private readonly MDFVVertexArray _vertexArray;
    private readonly MDFVVertexBuffer _vertexBuffer;
    private readonly MDFVIndexBuffer _indexBuffer;
    private readonly MDFVShaderProgram _fragmentVertexShaderProgram;
    private readonly MDFVFragmentUniforms _fragmentUniforms;
    private readonly MDFVShaderProgram _computeShaderProgram;
    private readonly MDFVComputeUniforms _computeUniforms;
    private uint _texture;
    
    public MDFVRenderer(MDFVContext context)
    {
        _context = context;
        
        _vertexArray = new MDFVVertexArray(_context.Gl);
        _vertexBuffer = new MDFVVertexBuffer(_context.Gl, MDFVConstants.Vertices);
        _indexBuffer = new MDFVIndexBuffer(_context.Gl, MDFVConstants.Indices);
        _fragmentVertexShaderProgram = new MDFVShaderProgram(_context.Gl);
        _computeShaderProgram = new MDFVShaderProgram(_context.Gl);
        
        ConfigureFragmentVertexShaderProgram();
        ConfigureComputeShaderProgram();
        
        _fragmentUniforms = new MDFVFragmentUniforms(_context.Gl, _fragmentVertexShaderProgram.GetId());
        _computeUniforms = new MDFVComputeUniforms(_context.Gl, _computeShaderProgram.GetId());
        
        ConfigureBuffers();
        CreateTexture();
        ComputeTexture();
    }

    private void ConfigureFragmentVertexShaderProgram()
    {
        var vertexShaderSource = MDFVShaderLoader.Load("Source/Shaders/default.vert");
        var fragmentShaderSource = MDFVShaderLoader.Load("Source/Shaders/default.frag");
        var vertexShader = new MDFVVertexShader(_context.Gl, vertexShaderSource);
        var fragmentShader = new MDFVFragmentShader(_context.Gl, fragmentShaderSource);
        
        _fragmentVertexShaderProgram.Attach(vertexShader);
        _fragmentVertexShaderProgram.Attach(fragmentShader);
        _fragmentVertexShaderProgram.Link();
        
        vertexShader.Delete();
        fragmentShader.Delete();
    }
    
    private void ConfigureComputeShaderProgram()
    {
        var computeShaderSource = MDFVShaderLoader.Load("Source/Shaders/default.comp");
        var computeShader = new MDFVComputeShader(_context.Gl, computeShaderSource);
        
        _computeShaderProgram.Attach(computeShader);
        _computeShaderProgram.Link();
        
        computeShader.Delete();
    }

    private unsafe void ConfigureBuffers()
    {
        _vertexArray.Bind();
        _vertexBuffer.Bind();
        _indexBuffer.Bind();
        
        _context.Gl.VertexAttribPointer(0, 3, GLEnum.Float, false, 0, null);
        _context.Gl.EnableVertexAttribArray(0);
        
        _vertexArray.Unbind(); 
        _vertexBuffer.Unbind();
    }
    
    public unsafe void Render()
    {
        _fragmentVertexShaderProgram.Use();
        _vertexArray.Bind();
        
        _context.Gl.ActiveTexture(TextureUnit.Texture0);
        _context.Gl.BindTexture(TextureTarget.Texture3D, _texture);
        
        _fragmentUniforms.SetUniforms(_context.Settings, _context.Camera);
        
        _context.Gl.DrawElements(GLEnum.Triangles, MDFVConstants.IndicesCount, GLEnum.UnsignedInt, null);
    }
    
    private unsafe void CreateTexture()
    {
        _texture = _context.Gl.GenTexture();
        _context.Gl.BindTexture(TextureTarget.Texture3D, _texture);
        _context.Gl.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
        _context.Gl.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
        _context.Gl.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapR, (int)GLEnum.ClampToEdge);
        _context.Gl.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMinFilter, (int)GLEnum.Nearest);
        _context.Gl.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMagFilter, (int)GLEnum.Nearest);
        
        var width = ((uint)_context.Settings.CurrentVoxelResolution + 31) / 32;
        _context.Gl.TexImage3D(
            TextureTarget.Texture3D,
            0,
            InternalFormat.R32ui,
            width,
            (uint)_context.Settings.CurrentVoxelResolution,
            (uint)_context.Settings.CurrentVoxelResolution * _context.Settings.CurrentSlicesCount,
            0,
            PixelFormat.RedInteger,
            PixelType.UnsignedInt,
            null
        );

        _context.Gl.BindTexture(TextureTarget.Texture3D, 0);
    }
    
    private void ComputeTexture()
    {
        _computeShaderProgram.Use();
        
        _context.Gl.BindImageTexture(
            0,
            _texture,
            0,
            true,
            0,
            GLEnum.WriteOnly,
            InternalFormat.R32ui
        );
        
        _computeUniforms.SetUniforms(_context.Settings);

        const int size = 8;
        var groups = (uint)((_context.Settings.CurrentVoxelResolution + size - 1) / size);
        _context.Gl.DispatchCompute(groups, groups, groups);
        _context.Gl.MemoryBarrier(MemoryBarrierMask.ShaderImageAccessBarrierBit | MemoryBarrierMask.TextureFetchBarrierBit);
    }

    public void RecomputeTexture()
    {
        CreateTexture();
        ComputeTexture();
    }

    public void Delete()
    {
        _vertexArray.Delete();
        _vertexBuffer.Delete();
        _indexBuffer.Delete();
        _fragmentVertexShaderProgram.Delete();
        _computeShaderProgram.Delete();
        _context.Gl.DeleteTexture(_texture);
    }
}