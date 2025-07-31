using FMOD.Studio;
using FMODUnity;
using Reflex.Attributes;
using UnityEngine;

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
    private void VolumeChanging(string busName, float volume)
    {
        if (RuntimeManager.GetBus("bus:/" + busName).isValid())
        {
            RuntimeManager.GetBus("bus:/" + busName).setVolume(volume);
        }
    }
    public void PlayOneShot(EventReference eventReference)
    {
        string eventPath = eventReference.Guid.ToString();
        RuntimeManager.PlayOneShot(eventPath);
    }
}
