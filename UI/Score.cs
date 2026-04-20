using Godot;
using System;

public partial class Score : Label
{
    public float score = 0;
    public float Total = 0;

    public override void _Ready()
    {
        Text = "Score: " + NodeExtension.FormatFloat(score);
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
