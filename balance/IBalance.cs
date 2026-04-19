namespace TreasureHunter.balance;

public interface IBalance
{
    BValue Balanced(UpgradeType type, int level);
    
    bool Checked(UpgradeType type, int level);
}