using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace StabQuest.Services;

public sealed class SoundService
{
    private static readonly SoundService _instance = new();

    private Song _backgroundSong1;
    private Song _backgroundSong2;
    private List<SoundEffect> _soundEffects;
    private bool _isBackgroundSong1Playing;
    private int stepCounter;

    static SoundService() { }

    private SoundService() { }

    public static SoundService Instance
    {
        get
        {
            return _instance;
        }
    }

    public ContentManager Content { get; set; }

    public enum SoundEffects
    {
        Sword,
        Step,
    }

    public void LoadSounds()
    {
        _backgroundSong1 = Content.Load<Song>("Sounds/Background1");
        _backgroundSong2 = Content.Load<Song>("Sounds/Background2");

        _soundEffects = new List<SoundEffect>
        {
            Content.Load<SoundEffect>("Sounds/sword sound"),
            Content.Load<SoundEffect>("Sounds/Step1"),
            Content.Load<SoundEffect>("Sounds/Step2"),
            Content.Load<SoundEffect>("Sounds/Step3")
        };
    }

    public void PlaySoundEffect(SoundEffects effect, float volume = 1, float pitch = 0, float pan = 0)
    {
        var effectToPlay = HandleMultiSourceEffects(effect);

        _soundEffects[effectToPlay].Play(volume, pitch, pan);
    }

    public void PlaySoundEffectInstance(SoundEffects effect, float volume = 1, float pitch = 0, float pan = 0)
    {
        var effectToPlay = HandleMultiSourceEffects(effect);

        var instance = _soundEffects[effectToPlay].CreateInstance();
        instance.Volume = volume;
        instance.Pitch = pitch;
        instance.Pan = pan;
        instance.Play();
    }

    public void PlayBackgroundMusic()
    {
        MediaPlayer.Play(_backgroundSong1);
        _isBackgroundSong1Playing = true;
        MediaPlayer.IsRepeating = true;
        MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
    }

    private void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
    {
        MediaPlayer.Play(_isBackgroundSong1Playing ? _backgroundSong2 : _backgroundSong1);
        _isBackgroundSong1Playing = !_isBackgroundSong1Playing;
    }

    private int HandleMultiSourceEffects(SoundEffects effect)
    {
        var effectToPlay = (int)effect;

        // Loop the 3 stepping sounds
        if (effect == SoundEffects.Step)
        {
            effectToPlay += stepCounter;
            stepCounter = (stepCounter + 1) % 3;
        }

        return effectToPlay;
    }
}