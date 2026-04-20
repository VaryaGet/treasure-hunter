using Godot;
using System;
using System.Linq;

public partial class Digger : Node2D
{
    [Export] public float Speed = 40;
    [Export] public float SpeedMult = 1;
    [Export] public int Damage = 6;
    public Timer DumbTimer { get; private set; }

    private Treasure _target;

    public override void _Ready()
    {
        DumbTimer = GetNode<Timer>("DumbTimer");
        SelectNewTarget();

        DumbTimer.Timeout += OnDumbTimerTimeout;
    }

    public override void _Process(double delta)
    {
        if (DumbTimer.IsStopped())
        {
            if (_target == null || !IsInstanceValid(_target))
            {
                SelectNewTarget();
            }

            if (_target != null)
            {
                var direction = (_target.GlobalPosition - GlobalPosition).Normalized();
                GlobalPosition += direction * Speed * SpeedMult * (float)delta;

                if (GlobalPosition.DistanceTo(_target.GlobalPosition) < 5f)
                {
                    _target.Hit(Damage);
                    DumbTimer.Start();
                }
            }
        }
    }

    private void OnDumbTimerTimeout()
    {
        SelectNewTarget();
    }

    private void SelectNewTarget()
    {
        _target = null;
        var treasures = GetTree().GetNodesInGroup(Groups.Treasure)
            .Select(n => (Treasure)n)
            .Where(t => !t.IsBusy && t.TreasureBody.IsFound);

        foreach (var treasure in treasures)
        {
            _target ??= treasure;

            if (treasure.GlobalPosition.DistanceTo(GlobalPosition) <
                _target.GlobalPosition.DistanceTo(GlobalPosition))
            {
                _target = treasure;
            }
        }

        if (_target != null)
        {
            _target.IsBusy = true;
        }
    }
}
