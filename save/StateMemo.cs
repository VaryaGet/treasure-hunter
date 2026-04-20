using System.Collections.Generic;
using System.Linq;
using TreasureHunter.balance;

namespace TreasureHunter.save;

public class StateMemo : IState
{
    private List<BState> states;
    private Dictionary<UpgradeType, BState> levels;

    public StateMemo(List<BState> states)
    {
        this.states = states;
        this.levels = states.GroupBy(s => s.Type)
            .ToDictionary(group => group.Key, group => group.First());
    }

    public int currentLevel(UpgradeType type)
    {
        return this.levels[type].Level;
    }

    public List<BState> current()
    {
        return this.states;
    }

    public void persist(UpgradeType type, int level)
    {
        this.levels[type].Level = level;
    }
}