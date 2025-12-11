namespace MDFV.Core.Source.Settings;

public class ScreenResolution
{
    public readonly int Width;
    public readonly int Height;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ScreenResolution(int width, int height)
    {
        Width = width;
        Height = height;
    }
}