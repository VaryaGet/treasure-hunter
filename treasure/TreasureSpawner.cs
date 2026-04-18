using Godot;
using System;

public partial class TreasureSpawner : Node2D
{
    [Export] public PackedScene treasure;
    [Export] public int count = 4;

    private Vector2 _min = Vector2.Zero;
    private Vector2 _max = Vector2.Zero;

    public override void _Ready()
    {
        _min = GetNode<Marker2D>("Area/Min").GlobalPosition;
        _max = GetNode<Marker2D>("Area/Max").GlobalPosition;

        for (var i = 0; i < count; i++)
        {
            var o = treasure.Instantiate<Node2D>();
            var pos = new Vector2((float)GD.RandRange(_min.X, _max.X), (float)GD.RandRange(_min.Y, _max.Y));
            o.GlobalPosition = pos;
            AddChild(o);
        }
    }
}
