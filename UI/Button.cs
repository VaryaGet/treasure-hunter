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
		this.state = godot.state;
		this.balance = godot.balance;
	}
	
	public override void _Process(double delta)
	{
	}

	public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
	{
		GD.Print(321);
		if (@event is InputEventMouseButton mouseEvent)
		{
			GD.Print(123);
			//check money amount
			int nextLevel = this.state.currentLevel(this.type) + 1;
			if (this.balance.Checked(this.type, nextLevel))
			{
				BValue nextValue = this.balance.Balanced(type, nextLevel);
				EmitSignalUpgraded(
					this.type,
					nextLevel,
					(float)nextValue.Value,
					(float)nextValue.Cost
				);
			}
		}
	}
}
