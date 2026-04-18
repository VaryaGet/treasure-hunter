using Godot;
using System;

public partial class Treasure : Node2D
{
    [Export] public int Hp = 6;
    [Export] public int Money = 2;

    [Signal]
    public delegate void DeadEventHandler(int money);

    public override void _Ready()
    {
        GetNode<StaticBody2D>("StaticBody2D").InputEvent += OnTreasureClicked;
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
        Hp -= damage;
        if (Hp <= 0)
        {
            QueueFree();
            EmitSignalDead(Money);
        }
    }
}
