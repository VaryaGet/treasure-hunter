using Godot;
using System;
using TreasureHunter.balance;
using TreasureHunter.save;

public partial class StateGd : Node2D
{
    public IState state;
    public IBalance balance;

    public override void _Ready()
    {
        var path = "res://resources/LD59.csv";

        GD.Print(FileAccess.FileExists(path));

        GD.Print(ResourceLoader.Exists(path));

        var text = FileAccess.GetFileAsString(path);
        GD.Print(text);
        // GD.Print(balancePath.ResourcePath);
        // GD.Print(FileAccess.Open(balancePath.ResourcePath, FileAccess.ModeFlags.Read));
        // balance = new Balance(path);
        // GD.Print(balance.Balanced(UpgradeType.DIGGER_QUANTITY, 1).Cost);
        // this.state = new StateCsv(AppDomain.CurrentDomain.BaseDirectory + "/saves");
    }

    public override void _Process(double delta)
    {
        // GD.Print(state.currentLevel(UpgradeType.DIGGER_QUANTITY));
    }
}
