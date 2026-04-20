using Godot;
using System;
using System.Linq;
using TreasureHunter.balance;
using TreasureHunter.helpers.digger;

public partial class DiggerSpawner : Node2D
{
    [Export] public PackedScene Digger;

    private PlayArea _playArea;
    private DiggerHolder _diggerHolder;

    public override void _Ready()
    {
        _playArea = this.GetPlayArea();
        this.GetStateGd().Ready += OnReady;
        SetProcess(false);
        //todo handle event and reset
    }

    private void OnReady()
    {
        _diggerHolder = new DiggerHolder(this.GetStateGd());
        SetProcess(true);
    }

    public override void _Process(double delta)
    {
        var count = GetTree().GetNodeCountInGroup(Groups.Diggers);
        if (count < _diggerHolder.Quantity)
        {
            CreateDigger();
        }
    }

    private void UpdateDiggers(UpgradeType type, int level, float value, float cost)
    {
        if (_diggerHolder.UpdateStates(type, value))
        {
            var diggers = GetTree().GetNodesInGroup(Groups.Diggers).OfType<Digger>();
            foreach (var digger in diggers)
            {
                digger.Speed = _diggerHolder.Speed;
                digger.DumbTimer.WaitTime = _diggerHolder.DigDelay;
            }
        }
    }

    private void CreateDigger()
    {
        var digger = Digger.Instantiate<Digger>();
        digger.GlobalPosition = _playArea.GetRandomPosition();
        AddChild(digger);
        digger.DumbTimer.WaitTime = _diggerHolder.DigDelay;
        digger.Speed = _diggerHolder.Speed;
    }
}
