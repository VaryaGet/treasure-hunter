using System;
using System.Collections.Generic;

namespace TreasureHunter.balance;

public class Balance : IBalance
{
    private Dictionary<UpgradeType, Dictionary<int, BValue>> _balances;

    public Balance(string path) : this(new BCsv().balanced(path))
    { }

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
            if (this.Balanced(type, level) != null)
            {
                if (this.Balanced(type, level).Cost !=0)
                {
                    return true;
                }
                if (level == 0 || level == 1)
                {
                    return true;
                }
            }
            return false;
        }
        catch (Exception exception)
        {
            return false;
        }
    }
}