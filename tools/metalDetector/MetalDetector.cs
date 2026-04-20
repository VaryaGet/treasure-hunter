using Godot;
using System;

public partial class MetalDetector : Node2D
{
    [Export] public float RadiusDivider = 2f;
    [Export] public float MaxSwayAngle = 15f;
    [Export] public float SwaySpeed = 8f;
    [Export] public float DampingSpeed = 5f;

    private PlayArea _playArea;
    private bool _isDragging = false;
    private Radar _radar;
    private Vector2 _dragOffset;
    private Vector2 _lastPosition;
    private Vector2 _currentVelocity;
    private float _currentSwayAngle = 0f;
    private float _targetSwayAngle = 0f;
    private bool _wasDragging = false;
    private bool isUp = false;

    public override void _Ready()
    {
        _playArea = this.GetPlayArea();
        _radar = GetNode<Radar>("Radar");
        _lastPosition = GlobalPosition;
        GetNode<StaticBody2D>("Body").InputEvent += OnInputEvent;
    }

    private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
        {
            _isDragging = true;
            _dragOffset = GlobalPosition - GetGlobalMousePosition();
            _currentVelocity = Vector2.Zero;
            isUp = GetGlobalMousePosition().Y > GetNode<CollisionShape2D>("Body/CollisionShape2D").GlobalPosition.Y;
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

        _currentVelocity = (clampedPos - _lastPosition) / (float)GetProcessDeltaTime();
        _lastPosition = clampedPos;

        GlobalPosition = clampedPos;
    }

    public override void _Process(double delta)
    {
        var deltaF = (float)delta;

        if (_isDragging)
        {
            UpdateSwayWhileDragging(deltaF);
            _wasDragging = true;
        }
        else
        {
            if (_wasDragging)
            {
                ApplyDampingAndReturn(deltaF);
            }
        }

        ApplySwayRotation();
    }

    private void UpdateSwayWhileDragging(float delta)
    {
        var direction = GetMovementDirection();

        var speedFactor = Mathf.Clamp(_currentVelocity.Length() / 500f, 0f, 1f);
        _targetSwayAngle = direction * MaxSwayAngle * speedFactor;

        _currentSwayAngle = Mathf.Lerp(_currentSwayAngle, _targetSwayAngle, SwaySpeed * delta);
    }

    private void ApplyDampingAndReturn(float delta)
    {
        _currentSwayAngle = Mathf.Lerp(_currentSwayAngle, 0f, DampingSpeed * delta);

        if (Mathf.Abs(_currentSwayAngle) < 0.01f)
        {
            _currentSwayAngle = 0f;
            _wasDragging = false;
            _currentVelocity = Vector2.Zero;
        }
    }

    private float GetMovementDirection()
    {
        if (_currentVelocity.Length() < 10f)
        {
            return 0f;
        }

        var direction = _currentVelocity.Normalized();

        if (isUp)
        {
            return -direction.X;
        }
        else
        {
            return direction.X;
        }
    }

    private void ApplySwayRotation()
    {
        Rotation = Mathf.DegToRad(_currentSwayAngle);
    }

    public bool IsInZone(Vector2 position)
    {
        return Geometry2D.IsPointInCircle(position, GlobalPosition, _radar.Radius / RadiusDivider);
    }
}
