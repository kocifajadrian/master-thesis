namespace MDFV.Core.Source;

public class MDFVApp
{
    private readonly MDFVContext _context;

    public MDFVApp()
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        _context = new MDFVContext();
        _context.Settings = new MDFVSettings();
        _context.Window = new MDFVWindow(_context);
    }

    public void Run()
    {
        _context.Window.Run();
    }
}