using Godot;
using System;

public partial class Treasure : Node2D
{
    [Export] public int Hp = 6;
    [Export] public int Money = 2;
    [Export] public ShaderMaterial Shader;

    [Signal]
    public delegate void DeadEventHandler(int money);

    private int _hp;

    public override void _Ready()
    {
        _hp = Hp;
        GetNode<StaticBody2D>("StaticBody2D").InputEvent += OnTreasureClicked;
        GetNode<Sprite2D>("Icon").Material = Shader;
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
        _hp -= damage;

        if (_hp <= 0)
        {
            QueueFree();
            EmitSignalDead(Money);
        }

        var value = 1 - Mathf.InverseLerp(0, Hp, _hp);
        Shader.SetShaderParameter("alpha", value);
    }
}
