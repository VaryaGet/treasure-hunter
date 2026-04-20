using Godot;
using System;

public partial class Treasure : Node2D
{
    [Export] public int Hp = 6;
    [Export] public float Money = 2;
    [Export] public Texture Shovel;

    public TreasureBody TreasureBody { get; private set; }
    public bool IsBusy { get; set; }
    public int CurrAnimation { get; set; }

    private Sprite2D _cross;
    private Sprite2D _shovel;
    private Sprite2D _grave;
    private AnimatedSprite2D _coin;
    private Label _label;
    private Timer _dieTimer;

    [Signal]
    public delegate void DeadEventHandler(float money);

    private int _hp;
    public bool _isDying { get; private set; }

    [Signal]
    public delegate void TreasureHitEventHandler(int damage);

    public override void _Ready()
    {
        _hp = Hp;
        TreasureBody = GetNode<TreasureBody>("TreasureBody");
        TreasureBody.InputEvent += OnTreasureClicked;
        TreasureBody.Found += CrossTreasure;

        _coin = GetNode<AnimatedSprite2D>("Coin");
        _grave = GetNode<Sprite2D>("Grave");
        _cross = GetNode<Sprite2D>("Cross");
        _shovel = GetNode<Sprite2D>("Shovel");
        _label = GetNode<Label>("Label");
        NewTreasure();

        _dieTimer = GetNode<Timer>("DieTimer");
        _dieTimer.Timeout += QueueFree;
        SetProcess(_isDying);
    }

    public override void _Process(double delta)
    {
        ((ShaderMaterial)_coin.Material).SetShaderParameter("t", _dieTimer.WaitTime - _dieTimer.TimeLeft);

        var val = Mathf.InverseLerp(0, _dieTimer.WaitTime, _dieTimer.TimeLeft);
        ((ShaderMaterial)_shovel.Material).SetShaderParameter("alpha", val);
        ((ShaderMaterial)_label.Material).SetShaderParameter("alpha", val);
        ((ShaderMaterial)_grave.Material).SetShaderParameter("alpha", val);
    }

    public void Hit(int damage)
    {
        OnTreasureHit(damage, false);
    }

    private void OnTreasureClicked(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true } && TreasureBody.IsFound)
        {
            OnTreasureHit(this.GetPlayer().Damage, true);
        }
    }

    private void OnTreasureHit(int damage, bool isPlayer)
    {
        if (!_isDying)
        {
            _hp -= damage;
            DiggingTreasure();

            if (_hp <= 0)
            {
                _isDying = true;
                _dieTimer.Start();
                CollectTreasure(isPlayer);
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
        _label.Hide();
        _shovel.Hide();
    }

    private void CrossTreasure()
    {
        _coin.Hide();
        _grave.Hide();
        _cross.Show();
        _label.Hide();
        _shovel.Hide();
    }

    private void DiggingTreasure()
    {
        _coin.Hide();
        _grave.Show();
        _cross.Hide();
        _label.Hide();
        _shovel.Hide();
    }

    private void CollectTreasure(bool isPlayer)
    {
        _coin.Show();
        _grave.Show();
        _cross.Hide();
        _label.Show();
        if (isPlayer)
        {
            _shovel.Show();
        }

        _label.Text = Money.ToString();

        _coin.SetAnimation(CurrAnimation.ToString());
        _coin.Play();
    }
}
