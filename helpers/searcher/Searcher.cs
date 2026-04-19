using Godot;
using System;

public partial class Searcher : Node2D
{
    [Export] public int Speed = 40;

    private PlayArea _playArea;
    private TreasureSpawner _treasureSpawner;
    public Timer WalkTimer { get; private set; }
    public Timer DumbTimer { get; private set; }

    private Vector2 _targetPoint;

    public override void _Ready()
    {
        _playArea = this.GetPlayArea();
        _treasureSpawner = this.GetTreasureSpawner();
        WalkTimer = GetNode<Timer>("WalkTimer");
        DumbTimer = GetNode<Timer>("DumbTimer");
        SelectNewTarget();

        DumbTimer.Timeout += OnDumbTimerTimeout;
        WalkTimer.Timeout += OnWalkTimerTimeout;
    }

    public override void _Process(double delta)
    {
        if (!WalkTimer.IsStopped())
        {
            var direction = (_targetPoint - GlobalPosition).Normalized();
            GlobalPosition += direction * Speed * (float)delta;

            if (GlobalPosition.DistanceTo(_targetPoint) < 5f)
            {
                SelectNewTarget();
            }
        }
    }

    private void OnDumbTimerTimeout()
    {
        SpawnTreasure();
        SelectNewTarget();
        WalkTimer.Start();
    }

    private void OnWalkTimerTimeout()
    {
        DumbTimer.Start();
    }

    private void SpawnTreasure()
    {
        _treasureSpawner.AddTreasure(GlobalPosition, true);
    }

    private void SelectNewTarget()
    {
        _targetPoint = _playArea.GetRandomPosition();
    }
}
