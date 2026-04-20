using Godot;
using System;
using System.Collections.Generic;
using TreasureHunter.balance;
using TreasureHunter.save;

public partial class Btn : StaticBody2D
{
    [Signal]
    public delegate void UpgradedEventHandler(UpgradeType type, int level, float value, float cost);

    [Export] public UpgradeType type { get; set; }

    public IState state;
    public IBalance balance;
    public Score score;
    public bool enabled;
    private StateGd stateGd;

    private Dictionary<UpgradeType, string> upgrades = new()
    {
        { UpgradeType.DIGGER_QUANTITY, "Diggers" },
        { UpgradeType.DIGGER_SHOVEL, "Digger power" },
        { UpgradeType.DIGGER_RUN, "Digger speed" },
        { UpgradeType.TREASURE_BRONSE, "Bronze value" },
        { UpgradeType.TREASURE_SILVER, "Silver value" },
        { UpgradeType.TREASURE_GOLD, "Gold value" },
        { UpgradeType.SEARCHER_QUANTITY, "Searchers" },
        { UpgradeType.SEARCHER_SEARCH, "Searcher speed" },
        { UpgradeType.SEARCHER_QUALITY, "Searcher skill" },
        { UpgradeType.TREASURE_TIER, "Coin tier" },
        { UpgradeType.QUALITY, "Detector quality" }
    };

    public override void _Ready()
    {
        stateGd = this.GetStateGd();
        state = stateGd.state;
        balance = stateGd.balance;
        InputEvent += MyInputEvent;
        score = this.GetScore();
        enabled = false;
        GetNode<LabelNextCost>("Labels/NextCost").init();
        GetNode<LabelNextLvl>("Labels/NextLvl").init();
    }

    public override void _Process(double delta)
    {
        if (!balance.Checked(type, state.currentLevel(type) + 1))
        {
            enabled = false;
        }
        else
        {
            enabled = (float)balance.Balanced(
                type,
                state.currentLevel(type) + 1
            ).Cost <= score.score;
        }
    }

    private void MyInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true } && enabled == true)
        {
            GD.Print(123);
            var nextLevel = state.currentLevel(type) + 1;
            if (balance.Checked(type, nextLevel))
            {
                GD.Print(nextLevel);
                var nextValue = balance.Balanced(type, nextLevel);
                state.persist(type, nextLevel);
                EmitSignalUpgraded(
                    type,
                    nextLevel,
                    (float)nextValue.Value,
                    (float)nextValue.Cost
                );
                score.AddScore(-1 * (float)nextValue.Cost);
            }
        }
    }

    public string Named()
    {
        return upgrades[type];
    }
}
