using Godot;
using System;

public partial class Score : Label
{
    public float score = 999999999999;
    public float Total = 0;

    public override void _Ready()
    {
        Text = "Score: " + score.ToString("N0");
    }

    public void AddScore(float value)
    {
        score += value;
        if (value > 0)
        {
            Total += value;
        }

        Text = "Score: " + NodeExtension.FormatFloat(score);
    }
}
