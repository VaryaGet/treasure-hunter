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
	public Score score;
	public bool enabled;

	public override void _Ready()
	{
		var godot = this.GetStateGd();
		state = godot.state;
		balance = godot.balance;
		InputEvent += MyInputEvent;
		score = this.GetScore();
		this.enabled = false;
	}

	public override void _Process(double delta)
	{
		this.enabled = (float)this.balance.Balanced(
			this.type,
			this.state.currentLevel(this.type) + 1
		).Cost <= this.score.score;
	}

	private void MyInputEvent(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true } && this.enabled == true)
		{
			GD.Print(123);
			// GD.Print("Upgrade cost:");
			// GD.Print(
			//     (float)this.balance.Balanced(
			//         this.type,
			//         this.state.currentLevel(this.type)
			//     ).Cost
			// );
			// GD.Print("")
			var nextLevel = state.currentLevel(type) + 1;
			if (balance.Checked(type, nextLevel))
			{
				GD.Print(nextLevel);
				var nextValue = balance.Balanced(type, nextLevel);
				this.state.persist(this.type, nextLevel);
				EmitSignalUpgraded(
					type,
					nextLevel,
					(float)nextValue.Value,
					(float)nextValue.Cost
				);
				this.score.AddScore(-1 * (float)nextValue.Cost);
			}
		}
	}
}
