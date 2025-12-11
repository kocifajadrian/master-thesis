namespace MDFV.Core.Source;

public class MDFVShaderLoader
{
    public static string Load(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"File not found: `{path}`.");
        }
        
        return File.ReadAllText(path);
    }
}