using Godot;
using System;

public partial class TreasureBody : StaticBody2D
{
    [Signal]
    public delegate void FoundEventHandler();

    public Treasure Treasure { get; private set; }
    public bool IsFound { get; private set; } = false;

    private Timer _findTimer;

    public override void _Ready()
    {
        Treasure = GetParent<Treasure>();
        _findTimer = GetNode<Timer>("FindTimer");
        _findTimer.Timeout += FoundTreasure;
    }

    public void CheckTimer(bool isInZone)
    {
        if (IsFound)
        {
            return;
        }

        if (isInZone)
        {
            if (_findTimer.IsStopped())
            {
                _findTimer.Start(_findTimer.WaitTime);
            }
        }
        else
        {
            _findTimer.Stop();
        }
    }

    public void FoundTreasure()
    {
        IsFound = true;
        EmitSignalFound();
    }
}
