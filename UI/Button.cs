using Godot;
using System;
using TreasureHunter.balance;
using TreasureHunter.save;

public partial class Button : StaticBody2D
{
    [Signal]
    public delegate void UpgradedEventHandler(UpgradeType type, int level, float value, float cost);

    [Export] public UpgradeType type { get; set; }

    public IState state;
    public IBalance balance;

    public override void _Ready()
    {
        var godot = this.GetStateGd();
        state = godot.state;
        balance = godot.balance;
        InputEvent += MyInputEvent;
    }

    public override void _Process(double delta)
    {
    }

    private void MyInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        GD.Print(321);
        if (@event is InputEventMouseButton mouseEvent)
        {
            GD.Print(123);
            //check money amount
            var nextLevel = state.currentLevel(type) + 1;
            if (balance.Checked(type, nextLevel))
            {
                var nextValue = balance.Balanced(type, nextLevel);
                EmitSignalUpgraded(
                    type,
                    nextLevel,
                    (float)nextValue.Value,
                    (float)nextValue.Cost
                );
            }
        }
    }
}
