using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace MDFV.Core.Source;

public class MDFVWindow
{
    private readonly MDFVContext _context;
    private readonly IWindow _window;

    public MDFVWindow(MDFVContext context)
    {
        _context = context;
        _window = Window.Create(GetOptions());
        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnRender;
    }

    private WindowOptions GetOptions()
    {
        var options = WindowOptions.Default;
        options.Title = MDFVConstants.WindowTitle;
        options.WindowBorder = WindowBorder.Hidden;
        options.Size  = new Vector2D<int>(
            _context.Settings.ScreenResolution.Width,
            _context.Settings.ScreenResolution.Height);
        return options;
    }
    
    private void OnLoad()
    {
        _context.Gl = _window.CreateOpenGL();
        _context.InputContext = _window.CreateInput();

        if (_context.InputContext.Keyboards.Count == 0) throw new ApplicationException("Keyboard does not exist.");
        if (_context.InputContext.Mice.Count == 0) throw new ApplicationException("Mouse does not exist.");

        _context.Keyboard = _context.InputContext.Keyboards[0];
        _context.Mouse = _context.InputContext.Mice[0];
        
        _context.Camera = new MDFVCamera(_context);
        _context.Renderer = new MDFVRenderer(_context);
        _context.Gui = new MDFVGraphicalUserInterface(_context, _window);
    }
    
    private void OnUpdate(double delta)
    {
        _context.Camera.Update((float)delta);
        _context.Gui.Update((float)delta);
    }
    
    private void OnRender(double delta)
    {
        _context.Gl.ClearColor(0f, 0f, 0f, 1f);
        _context.Gl.Clear((uint)ClearBufferMask.ColorBufferBit);
        
        _context.Renderer.Render();
        _context.Gui.Render();
    }
    
    public void Run()
    {
        _window.Run();
    }
}