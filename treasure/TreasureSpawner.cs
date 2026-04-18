using Godot;
using System;

public partial class TreasureSpawner : Node
{
    [Export] public PackedScene Treasure;
    [Export] public int MaxCount = 4;
    [Export] public Score ScoreLabel;
    [Export] public PlayArea PlayArea;

    public override void _Process(double delta)
    {
        var count = GetTree().GetNodeCountInGroup(Groups.Treasure);
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
        o.GlobalPosition = PlayArea.GetRandomPosition();
        AddChild(o);
        o.Dead += ScoreLabel.AddScore;
    }
}
