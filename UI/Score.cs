using Godot;
using System;

public partial class Score : Label
{
    private int _score = 0;

    public void AddScore(int value)
    {
        _score += value;
        Text = "Score: " + _score;
    }
}
