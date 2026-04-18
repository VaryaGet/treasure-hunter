using Godot;
using System;

public partial class TreasureSpawner : Node2D
{
    [Export] public PackedScene Treasure;
    [Export] public int MaxCount = 4;

    private Vector2 _min = Vector2.Zero;
    private Vector2 _max = Vector2.Zero;

    public override void _Ready()
    {
        _min = GetNode<Marker2D>("Area/Min").GlobalPosition;
        _max = GetNode<Marker2D>("Area/Max").GlobalPosition;

        for (var i = 0; i < MaxCount; i++)
        {
            AddTreasure();
        }
    }

    public override void _Process(double delta)
    {
        var count = GetTree().GetNodeCountInGroup("treasure");
        if (count < MaxCount)
        {
            for (var i = 0; i < MaxCount - count; i++)
            {
                AddTreasure();
            }
        }
    }

    private void AddTreasure()
    {
        var o = Treasure.Instantiate<Treasure>();
        var pos = new Vector2((float)GD.RandRange(_min.X, _max.X), (float)GD.RandRange(_min.Y, _max.Y));
        o.GlobalPosition = pos;
        AddChild(o);
    }
}
