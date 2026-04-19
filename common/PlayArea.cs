using Godot;
using System;

public partial class PlayArea : Area2D
{
    private Vector2 Min { get; set; }
    private Vector2 Max { get; set; }

    private RectangleShape2D _shape;

    public override void _Ready()
    {
        _shape = GetNode<CollisionShape2D>("Area").Shape as RectangleShape2D;
        Min = GlobalPosition - _shape.Size / 2;
        Max = GlobalPosition + _shape.Size / 2;
    }

    public Vector2 GetRandomPosition()
    {
        return new Vector2((float)GD.RandRange(Min.X, Max.X), (float)GD.RandRange(Min.Y, Max.Y));
    }

    public Vector2 GetClampedPosition(Vector2 position)
    {
        return new Vector2(
            Mathf.Clamp(position.X, Min.X, Max.X),
            Mathf.Clamp(position.Y, Min.Y, Max.Y)
        );
    }
}
