using Godot;
using System;

public partial class MetalDetector : Node2D
{
    [Export] public float RadiusDivider = 2f;
    private PlayArea _playArea;
    private bool _isDragging = false;
    private Radar _radar;
    private Vector2 _dragOffset;

    public override void _Ready()
    {
        _playArea = this.GetPlayArea();
        _radar = GetNode<Radar>("Radar");
        GetNode<StaticBody2D>("Body").InputEvent += OnInputEvent;
    }

    private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
        {
            _isDragging = true;
            _dragOffset = GlobalPosition - GetGlobalMousePosition();
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: false })
        {
            _isDragging = false;
        }

        if (_isDragging && @event is InputEventMouse)
        {
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        var mouseGlobalPos = GetGlobalMousePosition();
        var desiredPos = mouseGlobalPos + _dragOffset;
        var clampedPos = _playArea.GetClampedPosition(desiredPos);
        GlobalPosition = clampedPos;
    }

    public bool IsInZone(Vector2 position)
    {
        return Geometry2D.IsPointInCircle(position, GlobalPosition, _radar.Radius / RadiusDivider);
    }
}
