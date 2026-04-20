using Godot;
using System;

public partial class Score : Label
{
    private float _score = 0;
    private float _total = 0;

    public void AddScore(float value)
    {
        _score += value;
        if (value > 0)
        {
            _total += value;
        }

        Text = "Score: " + _score.ToString("N0");
    }
}
