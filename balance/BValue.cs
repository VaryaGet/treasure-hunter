namespace TreasureHunter.balance;

public class BValue
{
    public decimal Value { get; }
    public decimal Cost { get; }
    
    public BValue(decimal value, decimal cost)
    {
        Value = value;
        Cost = cost;
    }

    public override string ToString()
    {
        return $"Value: {Value}, Cost: {Cost}";
    }
}