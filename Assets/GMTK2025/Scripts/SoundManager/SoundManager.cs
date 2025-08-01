using FMOD.Studio;
using FMODUnity;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager
{
    [Inject]
    private SoundManagerConfig soundManagerConfig;
    public void ChangeVolume(VolumeType volumeType, float volume)
    {
        switch (volumeType)
        {
            case VolumeType.None:
                return;
            case VolumeType.Music:
                VolumeChanging(soundManagerConfig.musicBusName, volume);
                break;
            case VolumeType.Sfx:
                VolumeChanging(soundManagerConfig.vfxBusName, volume);
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
                VolumeGet(soundManagerConfig.musicBusName, slider);
                break;
            case VolumeType.Sfx:
                VolumeGet(soundManagerConfig.vfxBusName, slider);
                break;
        }
    }
    private void VolumeChanging(string busName, float volume)
    {
        if (RuntimeManager.GetBus("bus:/" + busName).isValid())
        {
            RuntimeManager.GetBus("bus:/" + busName).setVolume(volume);
        }
    }
    private void VolumeGet(string busName, UnityEngine.UI.Slider slider)
    {
        if (RuntimeManager.GetBus("bus:/" + busName).isValid())
        {
            RuntimeManager.GetBus("bus:/" + busName).getVolume(out float volume);
            slider.value = volume;
        }
    }
    public void PlayOneShot(EventReference eventReference)
    {
        string eventPath = eventReference.Guid.ToString();
        RuntimeManager.PlayOneShot(eventPath);
    }
    public void PlaySfxUiClick()
    {
        RuntimeManager.PlayOneShot(soundManagerConfig.sfxUiClickPath);
    }
    public void PlaySfxUiPointer()
    {
        RuntimeManager.PlayOneShot(soundManagerConfig.sfxUiPointerPath);
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
}
