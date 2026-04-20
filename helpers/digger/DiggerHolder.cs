using System;
using TreasureHunter.balance;

namespace TreasureHunter.helpers.digger;

public class DiggerHolder
{
    public int Quantity { get; private set; }
    public float DigDelay { get; private set; }
    public float Speed { get; private set; }

    public DiggerHolder(StateGd state)
    {
        FillStates(state);
    }

    public void FillStates(StateGd state)
    {
        foreach (var st in state.state.current())
        {
            UpdateStates(st.Type, state.balance.Balanced(st.Type, st.Level).Value);
        }
    }

    public void UpdateStates(UpgradeType upgradeType, decimal value)
    {
        switch (upgradeType)
        {
            case UpgradeType.DIGGER_QUANTITY:
                Quantity = (int)value;
                break;
            case UpgradeType.DIGGER_SHOVEL:
                DigDelay = (float)value;
                break;
            case UpgradeType.DIGGER_RUN:
                Speed = (float)value;
                break;
        }
    }
}
