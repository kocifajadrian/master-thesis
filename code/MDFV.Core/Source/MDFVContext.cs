using Silk.NET.Input;
using Silk.NET.OpenGL;

namespace MDFV.Core.Source;

public class MDFVContext
{
    public GL Gl = null!;
    public MDFVSettings Settings = null!;
    public MDFVWindow Window = null!;
    public MDFVRenderer Renderer = null!;
    public MDFVCamera Camera = null!;
    public MDFVGraphicalUserInterface Gui = null!;
    public IInputContext InputContext = null!;
    public IKeyboard Keyboard = null!;
    public IMouse Mouse = null!;
}