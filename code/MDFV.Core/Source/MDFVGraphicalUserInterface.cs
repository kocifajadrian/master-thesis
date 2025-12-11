using System.Numerics;
using ImGuiNET;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace MDFV.Core.Source;

public class MDFVGraphicalUserInterface
{
    private readonly MDFVContext _context;
    private readonly ImGuiController _gui;

    public MDFVGraphicalUserInterface(MDFVContext context, IWindow window)
    {
        _context = context;
        _gui = new ImGuiController(_context.Gl, window, _context.InputContext);
        DarkTheme();
    }
    
    public void Update(double delta)
    {
        _gui.Update((float)delta);
    }

    public void Render()
    {
        ImGui.Begin("Settings");
        BoundingBoxSettings();
        ImGui.Spacing();
        VoxelResolutionSettings();
        ImGui.Spacing();
        CameraSettings();
        ImGui.Spacing();
        SlicesSettings();
        ImGui.Spacing();
        Apply();
        
        ImGui.End();
        
        _gui.Render();
    }

    private void Apply()
    {
        if (ImGui.Button("Apply"))
        {
            _context.Settings.Apply();
            _context.Renderer.RecomputeTexture();
        }
    }
    
    private void BoundingBoxSettings()
    {
        var boundingBox = _context.Settings.NextBoundingBox;
        var boundingBoxEnd = boundingBox.Start + new Vector3(boundingBox.Size);
        ImGui.Text("Bounding Box");
        ImGui.InputFloat3("Start", ref boundingBox.Start);
        ImGui.BeginDisabled();
        ImGui.InputFloat3("End", ref boundingBoxEnd);
        ImGui.EndDisabled();
        ImGui.InputFloat("Size", ref boundingBox.Size);
        
        if (ImGui.Button("Reset Bounding Box"))
        {
            _context.Settings.ResetNextBoundingBox();
        }
    }
    
    private void VoxelResolutionSettings()
    {
        ImGui.Text("Voxel Resolution");
        ImGui.InputInt("Resolution", ref _context.Settings.NextVoxelResolution, 0, 0);

        if (ImGui.Button("Reset Voxel Resolution"))
        {
            _context.Settings.ResetNextVoxelResolution();
        }
    }
    
    private void CameraSettings()
    {
        var camera = _context.Camera;
        ImGui.Text("Camera");
        ImGui.InputFloat3("Position", ref camera.Position);
        ImGui.InputFloat("Speed", ref camera.Speed);

        if (ImGui.Button("Reset Camera"))
        {
            _context.Camera.ResetCamera();
        }
    }
    
    private void SlicesSettings()
    {
        ImGui.Text("Slices");

        for (uint i = 0; i < _context.Settings.NextSlicesCount; i++)
        {
            ImGui.InputFloat($"##Slice{i}", ref _context.Settings.NextSlices[i]);
            ImGui.SameLine();
            ImGui.ColorEdit3($"##SliceColor{i}", ref _context.Settings.NextSlicesColors[i], ImGuiColorEditFlags.NoInputs);
        }

        if (ImGui.Button("Add Slice"))
        {
            _context.Settings.AddNextSlice();
        }
        
        ImGui.SameLine();

        if (ImGui.Button("Reset Slices"))
        {
            _context.Settings.ResetNextSlices();
        }
    }

    private static void DarkTheme()
    {
        var style = ImGui.GetStyle();
        var colors = style.Colors;

        colors[(int)ImGuiCol.Text] = new Vector4(1f, 1f, 1f, 1f);
        colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.5f, 0.5f, 0.5f, 1f);
        colors[(int)ImGuiCol.WindowBg] = new Vector4(0.13f, 0.13f, 0.13f, 0.9f);
        colors[(int)ImGuiCol.ChildBg] = new Vector4(0f, 0f, 0f, 0f);
        colors[(int)ImGuiCol.PopupBg] = new Vector4(0.19f, 0.19f, 0.19f, 0.92f);
        colors[(int)ImGuiCol.Border] = new Vector4(0.19f, 0.19f, 0.19f, 0.29f);
        colors[(int)ImGuiCol.BorderShadow] = new Vector4(0f, 0f, 0f, 0.24f);
        colors[(int)ImGuiCol.FrameBg] = new Vector4(0.05f, 0.05f, 0.05f, 0.54f);
        colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.19f, 0.19f, 0.19f, 0.54f);
        colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.20f, 0.22f, 0.23f, 1f);
        colors[(int)ImGuiCol.TitleBg] = new Vector4(0f, 0f, 0f, 1f);
        colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.06f, 0.06f, 0.06f, 1f);
        colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0f, 0f, 0f, 1f);
        colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.14f, 0.14f, 0.14f, 1f);
        colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.05f, 0.05f, 0.05f, 0.54f);
        colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.34f, 0.34f, 0.34f, 0.54f);
        colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.4f, 0.4f, 0.4f, 0.54f);
        colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.56f, 0.56f, 0.56f, 0.54f);
        colors[(int)ImGuiCol.CheckMark] = new Vector4(0.33f, 0.67f, 0.86f, 1f);
        colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.34f, 0.34f, 0.34f, 0.54f);
        colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(0.56f, 0.56f, 0.56f, 0.54f);
        colors[(int)ImGuiCol.Button] = new Vector4(0.2f, 0.2f, 0.2f, 1f);
        colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.3f, 0.3f, 0.3f, 1f);
        colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.2f, 0.22f, 0.23f, 1f);
        
        style.WindowPadding = new Vector2(8f, 8f);
        style.FramePadding = new Vector2(6f, 3f);
        style.CellPadding = new Vector2(6f, 6f);
        style.ItemSpacing = new Vector2(6f, 6f);
        style.ItemInnerSpacing = new Vector2(6f, 6f);
        style.TouchExtraPadding = new Vector2(0f, 0f);
        style.IndentSpacing = 25f;
        style.ScrollbarSize = 15f;
        style.GrabMinSize = 10f;
        style.WindowBorderSize = 1f;
        style.ChildBorderSize = 1f;
        style.PopupBorderSize = 1f;
        style.FrameBorderSize = 1f;
        style.TabBorderSize = 1f;
        style.WindowRounding = 7f;
        style.ChildRounding = 4f;
        style.FrameRounding = 3f;
        style.PopupRounding = 4f;
        style.ScrollbarRounding = 9f;
        style.GrabRounding = 3f;
        style.LogSliderDeadzone = 4f;
        style.TabRounding = 4f;
    }
}