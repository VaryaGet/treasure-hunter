using System.Globalization;
using Godot;

public static class NodeExtension
{
    public static PlayArea GetPlayArea(this Node node)
    {
        return node.GetNode<PlayArea>("/root/Main/Common/PlayArea");
    }

    public static Shovel GetPlayer(this Node node)
    {
        return node.GetNode<Shovel>("/root/Main/Common/Shovel");
    }

    public static TreasureSpawner GetTreasureSpawner(this Node node)
    {
        return node.GetNode<TreasureSpawner>("/root/Main/Common/Holder/TreasureSpawner");
    }

    public static StateGd GetStateGd(this Node node)
    {
        return node.GetNode<StateGd>("/root/Main/StateGodot");
    }

    public static Score GetScore(this Node node)
    {
        return node.GetNode<Score>("/root/Main/Score");
    }

    public static string FormatFloat(float value)
    {
        return FormatDecimal((decimal)value);
    }

    public static string FormatDecimal(decimal value, int maxDecimals = 2)
    {
        if (value < 0)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        var thresholds = new[]
        {
            (Value: 1_000_000_000_000_000_000m, Suffix: "Qt", Divisor: 1_000_000_000_000_000_000m),
            (Value: 1_000_000_000_000_000m, Suffix: "Q", Divisor: 1_000_000_000_000_000m),
            (Value: 1_000_000_000_000m, Suffix: "T", Divisor: 1_000_000_000_000m),
            (Value: 1_000_000_000m, Suffix: "B", Divisor: 1_000_000_000m),
            (Value: 1_000_000m, Suffix: "M", Divisor: 1_000_000m),
            (Value: 1_000m, Suffix: "K", Divisor: 1_000m)
        };

        foreach (var threshold in thresholds)
        {
            if (value >= threshold.Value)
            {
                var shortened = value / threshold.Divisor;
                var format = $"0.{new string('#', maxDecimals)}";
                return shortened.ToString(format, CultureInfo.InvariantCulture) + threshold.Suffix;
            }
        }

        return value.ToString(CultureInfo.InvariantCulture);
    }
}
