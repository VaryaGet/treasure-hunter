using Godot;
using System;

public partial class MetalDetector : Node2D
{
    private PlayArea _playArea;
    private bool _isDragging = false;
    private Radar _radar;

    public override void _Ready()
    {
        _playArea = this.GetPlayArea();
        _radar = GetNode<Radar>("Radar");
        GetNode<StaticBody2D>("Body").InputEvent += OnInputEvent;
        GetViewport().GetVisibleRect();
    }

    private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
        {
            _isDragging = true;
            _radar.SetWork(_isDragging);
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: false })
        {
            _isDragging = false;
            _radar.SetWork(_isDragging);
        }

        if (_isDragging && @event is InputEventMouse)
        {
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        var mouseGlobalPos = GetGlobalMousePosition();
        var clampedPos = _playArea.GetClampedPosition(mouseGlobalPos);
        GlobalPosition = clampedPos;
    }
}
