using System;
using Reflex.Attributes;
using UnityEngine;

public class SetMusicType : MonoBehaviour
{
    [Inject]
    SoundManager soundManager;

    [SerializeField]
    MusicType musicType;

    private void Start()
    {
        soundManager.SetMusic(musicType.ToString());

        bool paused = musicType == MusicType.Menu;
        soundManager.PausedGameVolume(paused);
    }

    private enum MusicType
    {
        Game,
        Menu,
    }
}
