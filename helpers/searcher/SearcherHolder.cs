using System;
using Godot;
using TreasureHunter.balance;

namespace TreasureHunter.helpers.searcher;

public class SearcherHolder
{
    public int Quantity { get; private set; }
    public float SearchDelay { get; private set; }

    public SearcherHolder(StateGd state)
    {
        FillStates(state);
    }

    public void FillStates(StateGd state)
    {
        foreach (var st in state.state.current())
        {
            UpdateStates(st.Type, (float)state.balance.Balanced(st.Type, st.Level).Value);
        }
    }

    public bool UpdateStates(UpgradeType upgradeType, float value)
    {
        switch (upgradeType)
        {
            case UpgradeType.SEARCHER_QUANTITY:
                Quantity = (int)value;
                return false;
            case UpgradeType.SEARCHER_SEARCH:
                SearchDelay = value;
                return true;
            default:
                return false;
        }
    }
}
