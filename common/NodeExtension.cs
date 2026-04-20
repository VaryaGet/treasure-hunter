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
        if (value > 1000000)
        {
            // Формат с экспонентой, целочисленные нули после E+
            return value.ToString("0.##E+00", CultureInfo.InvariantCulture);
        }
        else
        {
            // Обычный формат для чисел <= 1000000
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }

    public static string FormatDecimal(decimal value)
    {
        if (value > 1000000)
        {
            // Формат с экспонентой, целочисленные нули после E+
            return value.ToString("0.##E+00", CultureInfo.InvariantCulture);
        }
        else
        {
            // Обычный формат для чисел <= 1000000
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
