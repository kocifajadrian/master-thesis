using System.Numerics;

namespace MDFV.Core.Source.Settings;

public class BoundingBox
{
    public Vector3 Start;
    public float Size;

    // ReSharper disable once ConvertToPrimaryConstructor
    public BoundingBox(Vector3 start, float size)
    {
        Start = start;
        Size = size;
    }
}