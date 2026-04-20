using System.Collections.Generic;
using TreasureHunter.balance;

namespace TreasureHunter.save;

public interface IState
{
    int currentLevel(UpgradeType type);
    List<BState> current();
    void persist(UpgradeType type, int level);
}