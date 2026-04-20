using Godot;
using System;
using TreasureHunter.balance;
using TreasureHunter.save;

public partial class StateGd : Node2D
{
	[Export]
	public Resource balancePath;
	public IState state;
	public IBalance balance;
	
	public override void _Ready()
	{
		GD.Print(balancePath.ResourceName);
		// GD.Print(FileAccess.Open(balancePath.ResourcePath, FileAccess.ModeFlags.Read));
		// this.balance = new Balance(balancePath.ResourcePath);
		// this.state = new StateCsv(AppDomain.CurrentDomain.BaseDirectory + "/saves");
	}

	public override void _Process(double delta)
	{
		// GD.Print(state.currentLevel(UpgradeType.DIGGER_QUANTITY));
	}
}
