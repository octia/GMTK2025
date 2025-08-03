using FMOD.Studio;
using FMODUnity;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager
{
    [Inject]
    public SoundManagerConfig soundManagerConfig;

    public void ChangeVolume(VolumeType volumeType, float volume)
    {
        switch (volumeType)
        {
            case VolumeType.None:
                return;
            case VolumeType.Music:
                VolumeChanging(soundManagerConfig.music_VCA, volume);
                break;
            case VolumeType.Sfx:
                VolumeChanging(soundManagerConfig.sfx_VCA, volume);
                break;
        }
    }

    public void PreResetVolume(VolumeType volumeType)
    {
        switch (volumeType)
        {
            case VolumeType.None:
                return;
            case VolumeType.Music:
                VolumeGet(soundManagerConfig.music_VCA, volumeType);
                break;
            case VolumeType.Sfx:
                VolumeGet(soundManagerConfig.sfx_VCA, volumeType);
                break;
        }
    }

    private void VolumeChanging(FMOD.Studio.VCA VCA, float volume)
    {
        if (VCA.isValid())
        {
            VCA.setVolume(volume);
        }
    }

    public void PausedGameVolume(bool bIsPaused)
    {
        if (bIsPaused)
        {
            RuntimeManager.StudioSystem.setParameterByName("Pause", 0f);
        }
        else
        {
            RuntimeManager.StudioSystem.setParameterByName("Pause", 100f);
        }
    }

    private void VolumeGet(FMOD.Studio.VCA VCA, VolumeType volumeType)
    {
        if (VCA.isValid())
        {
            VCA.getVolume(out float volume);

            switch (volumeType)
            {
                case VolumeType.None:
                    return;
                case VolumeType.Music:
                    soundManagerConfig.fVolumeMusic = volume;
                    break;
                case VolumeType.Sfx:
                    soundManagerConfig.fVolumeSfx = volume;
                    break;
            }
        }
    }

    public void PlayOneShot(string path)
    {
        RuntimeManager.PlayOneShot(path);
    }

    public EventInstance PlayLoop(string path, SizeType status, bool satelite)
    {
        EventInstance instance = RuntimeManager.CreateInstance(path);
        instance.setParameterByNameWithLabel("Shiptype", satelite ? "Satelite" : status.ToString());
        instance.start();
        soundManagerConfig.activeLoopedSounds.Add(instance);
        return instance;
    }

    public EventInstance PlayTextLoop()
    {
        EventInstance instance = RuntimeManager.CreateInstance(soundManagerConfig.sfxTextPath);
        instance.start();
        soundManagerConfig.activeLoopedSounds.Add(instance);
        return instance;
    }

    public void StopLoop(EventInstance instance)
    {
        if (instance.isValid())
        {
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instance.release();
            soundManagerConfig.activeLoopedSounds.Remove(instance);
        }
    }

    public void PlayOneShotWithStatus(string path, string status)
    {
        EventInstance instance = RuntimeManager.CreateInstance(path);
        instance.setParameterByNameWithLabel("Status", status);
        instance.start();
        instance.release();
    }

    public void PlayMusic(EventReference eventReference)
    {
        if (soundManagerConfig.musicLoopInstance.isValid())
        {
            soundManagerConfig.musicLoopInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            soundManagerConfig.musicLoopInstance.release();
        }
        soundManagerConfig.musicLoopInstance = RuntimeManager.CreateInstance(eventReference);
        soundManagerConfig.musicLoopInstance.start();
    }

    public void SetMusic(string type)
    {
        RuntimeManager.StudioSystem.setParameterByNameWithLabel("Gamestate", type);
    }

    public void UpdateMusicDistance(float normalizedDistance)
    {
        RuntimeManager.StudioSystem.setParameterByName("Distance", normalizedDistance * 100); //0 is near, 100 is far
    }
}
