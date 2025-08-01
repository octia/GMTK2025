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
    public void PlayOneShotWithStatus(EventReference eventReference, float status)
    {
        EventInstance instance = RuntimeManager.CreateInstance(eventReference);
        instance.setParameterByName("Status", status);
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
    public void UpdateMusicDistance(float distance, float maxDistance)
    {
        float volume = distance * 100 / maxDistance;
        RuntimeManager.StudioSystem.setParameterByName("Distance", volume); //0 is near, 100 is far
    }
}
