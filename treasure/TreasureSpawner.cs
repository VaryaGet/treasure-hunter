using System.Linq;
using Godot;
using TreasureHunter.balance;
using TreasureHunter.save;
using TreasureHunter.treasure;

public partial class TreasureSpawner : Node
{
    [Export] public PackedScene Treasure;
    [Export] public int MaxCount = 4;
    [Export] public Score ScoreLabel;
    [Export] public PlayArea PlayArea;
    [Export] public MetalDetector MetalDetector;

    private TreasureHolder _treasureHolder;

    public override void _Ready()
    {
        _treasureHolder = new TreasureHolder(this.GetStateGd());
        var btns = GetTree().GetNodesInGroup(Groups.UpgradesBtns).OfType<Btn>();
        foreach (var btn in btns)
        {
            btn.Upgraded += UpdateTreasure;
        }
    }

    private void UpdateTreasure(UpgradeType type, int level, float value, float cost)
    {
        _treasureHolder.UpdateStates(type, value);
    }

    public override void _Process(double delta)
    {
        var count = GetTree().GetNodeCountInGroup(Groups.PlayersTreasure);
        if (count < MaxCount)
        {
            for (var i = 0; i < MaxCount - count; i++)
            {
                AddTreasure(ChoosePosition(), false, true);
            }
        }
    }

    public void AddTreasure(Vector2 position, bool isFound, bool isForPlayer = false)
    {
        if (position != Vector2.Zero)
        {
            var o = Treasure.Instantiate<Treasure>();
            o.GlobalPosition = position;
            AddChild(o);
            o.Dead += ScoreLabel.AddScore;

            var type = _treasureHolder.GetRandomTreasure(isForPlayer);
            o.CurrAnimation = type.Id;
            o.Money = type.Income;
            if (isFound)
            {
                o.TreasureBody.FoundTreasure();
            }

            if (isForPlayer)
            {
                o.AddToGroup(Groups.PlayersTreasure);
            }
        }
    }

    private Vector2 ChoosePosition()
    {
        var result = Vector2.Zero;
        for (var i = 0; i < 3; i++)
        {
            var pos = PlayArea.GetRandomPosition();
            if (!MetalDetector.IsInZone(pos))
            {
                result = pos;
                break;
            }
        }

        return result;
    }
}
