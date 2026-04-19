using Godot;
using System;

public partial class TreasureSpawner : Node
{
    [Export] public PackedScene Treasure;
    [Export] public int MaxCount = 4;
    [Export] public Score ScoreLabel;
    [Export] public PlayArea PlayArea;
    [Export] public MetalDetector MetalDetector;

    public override void _Process(double delta)
    {
        var count = GetTree().GetNodeCountInGroup(Groups.Treasure);
        if (count < MaxCount)
        {
            for (var i = 0; i < MaxCount - count; i++)
            {
                AddTreasure(ChoosePosition(), false);
            }
        }
    }

    public void AddTreasure(Vector2 position, bool isFound)
    {
        if (position != Vector2.Zero)
        {
            var o = Treasure.Instantiate<Treasure>();
            o.GlobalPosition = position;
            AddChild(o);
            o.Dead += ScoreLabel.AddScore;
            if (isFound)
            {
                o.TreasureBody.FoundTreasure();
            }
        }
    }

    private Vector2 ChoosePosition()
    {
        var result = Vector2.Zero;
        for (var i = 0; i < 3; i++)
        {
            var pos = PlayArea.GetRandomPosition();
            if (!MetalDetector.IsInZone(pos))
            {
                result = pos;
                break;
            }
        }

        return result;
    }
}
