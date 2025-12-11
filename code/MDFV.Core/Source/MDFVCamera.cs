using System.Numerics;
using Silk.NET.Input;

namespace MDFV.Core.Source;

public class MDFVCamera
{
    private readonly MDFVContext _context;
    public Vector3 Position;
    public Vector3 Forward;
    public Vector3 Right;
    public Vector3 Up;
    public float Fov;
    public float Yaw;
    public float Pitch;
    public float Speed;
    public float Sensitivity;
    public float Zoom;
    public bool MousePressed;
    public Vector2 MouseLastPosition;
    
    public MDFVCamera(MDFVContext context)
    {
        _context = context;
        ResetCamera();
        
        _context.Mouse.Scroll += OnScroll;
        _context.Mouse.MouseDown += OnMouseDown;
        _context.Mouse.MouseUp += OnMouseUp;
        _context.Mouse.MouseMove += OnMouseMove;
    }
    
    public void ResetCamera()
    {
        Position = MDFVConstants.DefaultCameraPosition;
        Forward = MDFVConstants.DefaultCameraForward;
        Right = MDFVConstants.DefaultCameraRight;
        Up = MDFVConstants.DefaultCameraUp;
        Fov = MDFVConstants.DefaultCameraFov;
        Yaw = MDFVConstants.DefaultCameraYaw;
        Pitch = MDFVConstants.DefaultCameraPitch;
        Speed = MDFVConstants.DefaultCameraSpeed;
        Sensitivity = MDFVConstants.DefaultCameraSensitivity;
        Zoom = MDFVConstants.DefaultCameraZoom;
        MousePressed = MDFVConstants.DefaultMouseClick;
        MouseLastPosition = MDFVConstants.DefaultMouseLastPosition;
    }
    
    public void Update(float delta)
    {
        var zoomFactor = Fov / MDFVConstants.DefaultCameraFov;
        var amount = Speed * delta * zoomFactor;

        if (_context.Keyboard.IsKeyPressed(Key.W)) Position += Up * amount;
        if (_context.Keyboard.IsKeyPressed(Key.A)) Position -= Right * amount;
        if (_context.Keyboard.IsKeyPressed(Key.S)) Position -= Up * amount;
        if (_context.Keyboard.IsKeyPressed(Key.D)) Position += Right * amount;
        if (!_context.Keyboard.IsKeyPressed(Key.ControlLeft)) Fov = MDFVConstants.DefaultCameraFov;
    }
    
    private void OnMouseMove(IMouse mouse, Vector2 position)
    {
        if (!MousePressed) return;

        var deltaX = position.X - MouseLastPosition.X;
        var deltaY = MouseLastPosition.Y - position.Y;
        var zoomFactor = Fov / MDFVConstants.DefaultCameraFov;

        MouseLastPosition = new Vector2(position.X, position.Y);
        Yaw += Sensitivity * deltaX * zoomFactor / _context.Settings.ScreenResolution.Width;
        Pitch += Sensitivity * deltaY * zoomFactor / _context.Settings.ScreenResolution.Height;

        if (Pitch > MDFVConstants.MaximumCameraPitch) Pitch = MDFVConstants.MaximumCameraPitch;
        if (Pitch < MDFVConstants.MinimumCameraPitch) Pitch = MDFVConstants.MinimumCameraPitch;
        
        UpdateVectors();
    }

    private void OnMouseDown(IMouse mouse, MouseButton button)
    {
        if (button != MouseButton.Right) return;
        
        MousePressed = true;
        mouse.Cursor.CursorMode = CursorMode.Raw;
        MouseLastPosition = new Vector2(mouse.Position.X, mouse.Position.Y);
    }

    private void OnMouseUp(IMouse mouse, MouseButton button)
    {
        if (button != MouseButton.Right) return;
        
        MousePressed = false;
        mouse.Cursor.CursorMode = CursorMode.Normal;
    }
    
    private void OnScroll(IMouse mouse, ScrollWheel scrollWheel)
    {
        if (_context.Keyboard.IsKeyPressed(Key.ControlLeft))
        {
            if (scrollWheel.Y > 0)
            {
                Fov *= MDFVConstants.DefaultCameraFovMultiplier;
            }
        }
        else
        {
            Position += Forward * scrollWheel.Y * Zoom * Speed;
        }
    }
    
    private void UpdateVectors()
    {
        var radiansYaw = Yaw * (MathF.PI / 180f);
        var radiansPitch = Pitch * (MathF.PI / 180f);

        Forward = Vector3.Normalize(
            new Vector3(
                MathF.Cos(radiansYaw) * MathF.Cos(radiansPitch),
                MathF.Sin(radiansPitch),
                MathF.Sin(radiansYaw) * MathF.Cos(radiansPitch)
        ));

        Right = Vector3.Normalize(Vector3.Cross(Forward, MDFVConstants.DefaultCameraWorldUp));
        Up = Vector3.Normalize(Vector3.Cross(Right, Forward));
    }
}