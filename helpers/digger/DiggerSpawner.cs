using Godot;
using System;

public partial class DiggerSpawner : Node2D
{
    [Export] public PackedScene Digger;

    private PlayArea _playArea;

    public override void _Ready()
    {
        _playArea = this.GetPlayArea();
    }

    private void CreateDigger()
    {
        var digger = Digger.Instantiate<Digger>();
        digger.GlobalPosition = _playArea.GetRandomPosition();
        AddChild(digger);
    }
}
