using Godot;
using System;

public partial class Music : Node
{
    private AudioStreamPlayer introPlayer;
    private AudioStreamPlayer musicPlayer;

    private int musicBusIndex;
    private int introBusIndex;

    public override void _Ready()
    {
        introPlayer = GetNode<AudioStreamPlayer>("IntroPlayer");
        musicPlayer = GetNode<AudioStreamPlayer>("MusicPlayer");

        musicBusIndex = AudioServer.GetBusIndex("Music");
        introBusIndex = AudioServer.GetBusIndex("Effect");

        PlayGameAudio();
    }

    public void PlayGameAudio()
    {
        introPlayer.Play();
        introPlayer.Finished += OnIntroFinished;
    }

    private void OnIntroFinished()
    {
        musicPlayer.Play();
    }

    // Метод для ползунка (Range) со значениями от 0 до 1 (Linear scale)
    public void OnMusicVolumeSliderChanged(float value)
    {
        var volumeDB = Mathf.LinearToDb(value);
        AudioServer.SetBusVolumeDb(musicBusIndex, volumeDB);
    }
}
