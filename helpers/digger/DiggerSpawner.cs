using Godot;
using System;
using System.Linq;
using TreasureHunter.balance;
using TreasureHunter.helpers.digger;

public partial class DiggerSpawner : Node2D
{
	[Export] public PackedScene Digger;

	private PlayArea _playArea;
	private DiggerHolder _diggerHolder;

	public override void _Ready()
	{
		_playArea = this.GetPlayArea();
		_diggerHolder = new DiggerHolder(this.GetStateGd());
		var btns = GetTree().GetNodesInGroup(Groups.UpgradesBtns).OfType<Btn>();
		foreach (var btn in btns)
		{
			btn.Upgraded += UpdateDiggers;
		}
	}

	public override void _Process(double delta)
	{
		var count = GetTree().GetNodeCountInGroup(Groups.Diggers);
		if (count < _diggerHolder.Quantity)
		{
			CreateDigger();
		}
	}

	private void UpdateDiggers(UpgradeType type, int level, float value, float cost)
	{
		if (_diggerHolder.UpdateStates(type, value))
		{
			var diggers = GetTree().GetNodesInGroup(Groups.Diggers).OfType<Digger>();
			foreach (var digger in diggers)
			{
				digger.Speed = _diggerHolder.Speed;
				digger.DumbTimer.WaitTime = _diggerHolder.DigDelay;
			}
		}
	}

	private void CreateDigger()
	{
		var digger = Digger.Instantiate<Digger>();
		digger.GlobalPosition = _playArea.GetRandomPosition();
		AddChild(digger);
		digger.DumbTimer.WaitTime = _diggerHolder.DigDelay;
		digger.Speed = _diggerHolder.Speed;
	}
}
