using TreasureHunter.balance;

namespace TreasureHunter.save;

public class BState
{
    public int Level { get; set; }
    public UpgradeType Type { get; }

    public BState(int level, UpgradeType type)
    {
        Level = level;
        Type = type;
    }
}
