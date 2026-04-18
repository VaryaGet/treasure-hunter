using Godot;
using System;

public partial class Treasure : Node2D
{
    [Export] public int hp = 6;

    [Signal]
    public delegate void DeadEventHandler();

    private int _hp;

    public override void _Ready()
    {
        _hp = hp;
        GetNode<StaticBody2D>("StaticBody2D").InputEvent += OnTreasureClicked;
    }

    private void OnTreasureClicked(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
        {
            OnTreasureHit(GetTree().Root.GetNode<Player>("Main/Player").Damage);
        }
    }

    private void OnTreasureHit(int damage)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            QueueFree();
            EmitSignalDead();
        }
    }
}
