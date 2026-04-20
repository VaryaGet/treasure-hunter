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
}
