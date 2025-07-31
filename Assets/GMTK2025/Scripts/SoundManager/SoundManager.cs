using FMODUnity;
using Reflex.Attributes;

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
                ChangeVolume(soundManagerConfig.musicBusName, volume);
                break;
            case VolumeType.Sfx:
                ChangeVolume(soundManagerConfig.vfxBusName, volume);
                break;
        }
    }
    private void ChangeVolume(string busName, float volume)
    {
        if (RuntimeManager.GetBus("bus:/" + busName).isValid())
        {
            RuntimeManager.GetBus("bus:/" + busName).setVolume(volume);
        }
    }
}
