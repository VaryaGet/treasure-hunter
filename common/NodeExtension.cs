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
}
