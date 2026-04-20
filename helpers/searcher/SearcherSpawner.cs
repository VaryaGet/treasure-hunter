using Godot;
using System;
using System.Linq;
using TreasureHunter.balance;
using TreasureHunter.helpers.searcher;

public partial class SearcherSpawner : Node2D
{
	[Export] public PackedScene Searcher;

	private PlayArea _playArea;
	private SearcherHolder _searcherHolder;

	public override void _Ready()
	{
		_playArea = this.GetPlayArea();
		_searcherHolder = new SearcherHolder(this.GetStateGd());
		var btns = GetTree().GetNodesInGroup(Groups.UpgradesBtns).OfType<Btn>();
		foreach (var btn in btns)
		{
			btn.Upgraded += UpdateSearchers;
		}
	}

	public override void _Process(double delta)
	{
		var count = GetTree().GetNodeCountInGroup(Groups.Searchers);
		if (count < _searcherHolder.Quantity)
		{
			CreateSearcher();
		}
	}

	private void UpdateSearchers(UpgradeType type, int level, float value, float cost)
	{
		if (_searcherHolder.UpdateStates(type, value))
		{
			var searchers = GetTree().GetNodesInGroup(Groups.Searchers).OfType<Searcher>();
			foreach (var searcher in searchers)
			{
				searcher.DumbTimer.WaitTime = _searcherHolder.SearchDelay;
				searcher.WalkTimer.WaitTime = _searcherHolder.SearchDelay;
			}
		}
	}

	private void CreateSearcher()
	{
		var searcher = Searcher.Instantiate<Searcher>();
		searcher.GlobalPosition = _playArea.GetRandomPosition();
		AddChild(searcher);
		searcher.DumbTimer.WaitTime = _searcherHolder.SearchDelay;
		searcher.WalkTimer.WaitTime = _searcherHolder.SearchDelay;
	}
}
