using Godot;

public static class NodeExtension
{
    public static PlayArea GetPlayArea(this Node node)
    {
        return node.GetNode<PlayArea>("/root/Main/Common/PlayArea");
    }

    public static Player GetPlayer(this Node node)
    {
        return node.GetNode<Player>("/root/Main/Common/Player");
    }
}
