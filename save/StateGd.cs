using Godot;
using System;
using System.IO;
using TreasureHunter.balance;
using TreasureHunter.save;
using FileAccess = Godot.FileAccess;

public partial class StateGd : Node2D
{
    public IState state;
    public IBalance balance;

    [Signal]
    public delegate void IsReadyEventHandler();

    public override void _Ready()
    {
        var text = FileAccess.GetFileAsString("res://resources/LD59.csv");
        File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/balance.csv", text);
        balance = new Balance(AppDomain.CurrentDomain.BaseDirectory + "/balance.csv");
        state = new StateCsv(AppDomain.CurrentDomain.BaseDirectory + "/saves");
        EmitSignalIsReady();
    }

    public override void _Process(double delta)
    {
        // GD.Print(state.currentLevel(UpgradeType.DIGGER_QUANTITY));
    }
}
