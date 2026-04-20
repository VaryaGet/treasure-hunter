using Godot;
using System;

public partial class Score : Label
{
    public float score = 0;
    public float Total = 0;

    public override void _Ready()
    {
        AddThemeColorOverride("font_color", new Color(0, 0, 0));
    }

    public void AddScore(float value)
    {
        score += value;
        if (value > 0)
        {
            Total += value;
        }

        Text = "Score: " + score.ToString("N0");
    }
}
