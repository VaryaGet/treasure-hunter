using Godot;
using System;

public partial class Treasure : Node2D
{
    [Export] public int Hp = 6;
    [Export] public int Money = 2;

    public TreasureBody TreasureBody { get; private set; }
    private Sprite2D _cross;
    private Sprite2D _grave;
    private Sprite2D _coin;
    private Timer _dieTimer;

    [Signal]
    public delegate void DeadEventHandler(int money);

    private int _hp;
    private bool _isDying = false;

    public override void _Ready()
    {
        _hp = Hp;
        TreasureBody = GetNode<TreasureBody>("TreasureBody");
        TreasureBody.InputEvent += OnTreasureClicked;
        TreasureBody.Found += CrossTreasure;

        _coin = GetNode<Sprite2D>("Coin");
        _grave = GetNode<Sprite2D>("Grave");
        _cross = GetNode<Sprite2D>("Cross");
        NewTreasure();

        _dieTimer = GetNode<Timer>("DieTimer");
        _dieTimer.Timeout += QueueFree;
        SetProcess(_isDying);
    }

    public override void _Process(double delta)
    {
        var shader = (ShaderMaterial)_coin.Material;
        shader.SetShaderParameter("t", _dieTimer.WaitTime - _dieTimer.TimeLeft);
    }

    private void OnTreasureClicked(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
        {
            OnTreasureHit(this.GetPlayer().Damage);
        }
    }

    private void OnTreasureHit(int damage)
    {
        if (!_isDying)
        {
            _hp -= damage;
            DiggingTreasure();

            if (_hp <= 0)
            {
                _isDying = true;
                _dieTimer.Start();
                CollectTreasure();
                SetProcess(_isDying);
                EmitSignalDead(Money);
            }
        }
    }

    private void NewTreasure()
    {
        _coin.Hide();
        _grave.Hide();
        _cross.Hide();
    }

    private void CrossTreasure()
    {
        _coin.Hide();
        _grave.Hide();
        _cross.Show();
        RemoveFromGroup(Groups.PlayersTreasure);
    }

    private void DiggingTreasure()
    {
        _coin.Hide();
        _grave.Show();
        _cross.Hide();
    }

    private void CollectTreasure()
    {
        _coin.Show();
        _grave.Show();
        _cross.Hide();
    }
}
