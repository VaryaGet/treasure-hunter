using System;
using System.Collections.Generic;

namespace TreasureHunter.balance;

public class Balance : IBalance
{
    private Dictionary<UpgradeType, Dictionary<int, BValue>> _balances;

    public Balance(Dictionary<UpgradeType, Dictionary<int, BValue>> balances)
    {
        this._balances = balances;
    }

    public BValue Balanced(UpgradeType type, int level)
    {
        return _balances[type][level];
    }

    public bool Checked(UpgradeType type, int level)
    {
        try
        {
            return this.Balanced(type, level) != null;
        }
        catch (Exception exception)
        {
            return false;
        }
    }
}