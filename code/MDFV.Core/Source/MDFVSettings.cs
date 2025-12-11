using System.Numerics;
using MDFV.Core.Source.Settings;

namespace MDFV.Core.Source;

public class MDFVSettings
{
    public readonly ScreenResolution ScreenResolution;
    public BoundingBox CurrentBoundingBox;
    public BoundingBox NextBoundingBox;
    public int CurrentVoxelResolution;
    public int NextVoxelResolution;
    public uint CurrentSlicesCount;
    public uint NextSlicesCount;
    public float[] CurrentSlices;
    public float[] NextSlices;
    public Vector3[] CurrentSlicesColors;
    public Vector3[] NextSlicesColors;

    // ReSharper disable once ConvertConstructorToMemberInitializers
    public MDFVSettings()
    {
        ScreenResolution = new ScreenResolution(
            MDFVConstants.DefaultScreenResolutionWidth,
            MDFVConstants.DefaultScreenResolutionHeight);
        
        CurrentBoundingBox = new BoundingBox(
            MDFVConstants.DefaultBoundingBoxStart,
            MDFVConstants.DefaultBoundingBoxSize);
        
        NextBoundingBox = new BoundingBox(
            MDFVConstants.DefaultBoundingBoxStart,
            MDFVConstants.DefaultBoundingBoxSize);
        
        CurrentVoxelResolution = MDFVConstants.DefaultVoxelResolution;
        NextVoxelResolution = MDFVConstants.DefaultVoxelResolution;
        CurrentSlicesCount = MDFVConstants.DefaultSlicesCount;
        NextSlicesCount = MDFVConstants.DefaultSlicesCount;
        CurrentSlices = (float[])MDFVConstants.DefaultSlices.Clone();
        NextSlices = (float[])MDFVConstants.DefaultSlices.Clone();
        CurrentSlicesColors = (Vector3[])MDFVConstants.DefaultSlicesColors.Clone();
        NextSlicesColors = (Vector3[])MDFVConstants.DefaultSlicesColors.Clone();
    }
    
    public void Apply()
    {
        CurrentBoundingBox = new BoundingBox(
            NextBoundingBox.Start, 
            NextBoundingBox.Size
        );
        
        CurrentVoxelResolution = NextVoxelResolution;
        CurrentSlicesCount = NextSlicesCount;
        CurrentSlices = (float[])NextSlices.Clone();
        CurrentSlicesColors = (Vector3[])NextSlicesColors.Clone();
    }

    public void ResetNextBoundingBox()
    {
        NextBoundingBox = new BoundingBox(
            MDFVConstants.DefaultBoundingBoxStart,
            MDFVConstants.DefaultBoundingBoxSize);
    }

    public void ResetNextVoxelResolution()
    {
        NextVoxelResolution = MDFVConstants.DefaultVoxelResolution;
    }
    
    public void AddNextSlice()
    {
        Array.Resize(ref NextSlices, (int)(NextSlicesCount + 1));
        Array.Resize(ref NextSlicesColors, (int)(NextSlicesCount + 1));
        NextSlicesCount += 1;
        NextSlices[NextSlicesCount - 1] = MDFVConstants.DefaultSlice;
        NextSlicesColors[NextSlicesCount - 1] = MDFVConstants.DefaultSliceColor;
    }

    public void ResetNextSlices()
    {
        NextSlicesCount = MDFVConstants.DefaultSlicesCount;
        NextSlices = (float[])MDFVConstants.DefaultSlices.Clone();
        NextSlicesColors = (Vector3[])MDFVConstants.DefaultSlicesColors.Clone();
    }
}