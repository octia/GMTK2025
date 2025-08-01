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
    public void ResetVolume(VolumeType volumeType, UnityEngine.UI.Slider slider)
    {
        switch (volumeType)
        {
            case VolumeType.None:
                return;
            case VolumeType.Music:
                VolumeGet(soundManagerConfig.music_VCA, slider);
                break;
            case VolumeType.Sfx:
                VolumeGet(soundManagerConfig.sfx_VCA, slider);
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
        if(bIsPaused)
        {
            RuntimeManager.StudioSystem.setParameterByName("Pause", 0f);
        }
        else
        {
            RuntimeManager.StudioSystem.setParameterByName("Pause", 100f);
        }
    }
    private void VolumeGet(FMOD.Studio.VCA VCA, UnityEngine.UI.Slider slider)
    {
        if (VCA.isValid())
        {
            VCA.getVolume(out float volume);
            slider.value = volume;
        }
    }
    public void PlayOneShot(string path)
    {
        RuntimeManager.PlayOneShot(path);
    }
    public EventInstance PlayLoop(string path, string status_)
    {
        (float status, string paramName) = FindStatus(status_);
        EventInstance instance = RuntimeManager.CreateInstance(path);
        instance.setParameterByName(paramName, status);
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
    public void PlayOneShotWithStatus(EventReference eventReference, string status_)
    {
        (float status, string paramName) = FindStatus(status_);
        EventInstance instance = RuntimeManager.CreateInstance(eventReference);
        instance.setParameterByName(paramName, status);
        instance.start();
        instance.release();
    }
    private (float, string) FindStatus(string status_)
    {
        if(status_ == "satelite" || status_ == "Satelite")
        {
            return (3f, "ShipType");
        }
        else if(status_ == "big" || status_ == "Big")
        {
            return (2f, "ShipType");
        }
        else if (status_ == "mid" || status_ == "Mid")
        {
            return (1f, "ShipType");
        }
        else if (status_ == "small" || status_ == "Small")
        {
            return (0f, "ShipType");
        }
        else
        {
            return (0f, "Status");
        }
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
    public void UpdateMusicDistance(float distance, float maxDistance)
    {
        float volume = distance * 100 / maxDistance;
        RuntimeManager.StudioSystem.setParameterByName("Distance", volume); //0 is near, 100 is far
    }
}
