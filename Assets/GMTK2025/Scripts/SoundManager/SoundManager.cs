using Reflex.Attributes;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private VolumeType volumeType;

    [Inject]
    private SoundManagerConfig soundManager;

     private UnityEngine.UI.Slider thisSlider;
    private void Start()
    {
        if (!soundManager.music_VCA.isValid() || !soundManager.sfx_VCA.isValid())
        {
            Debug.LogWarning("VCA is not valid! Check FMOD path.");
        }
        thisSlider = transform.GetComponent<UnityEngine.UI.Slider>();
    }
    public void VolumeChange()
    {
        ChangeVolume(volumeType);
    }
    private void ChangeVolume(VolumeType volumeType)
    {
        switch (volumeType)
        {
            case VolumeType.None:
                return;
            case VolumeType.Music:
                ChangeVolumeMusic();
                break;
            case VolumeType.Sfx:
                ChangeVolumeSfx();
                break;
        }
    }

    private void ChangeVolumeMusic()
    {
        if (!soundManager.music_VCA.isValid())
        {
            Debug.LogWarning("VCA is not valid! Check FMOD path.");
        }
        else
        {
            soundManager.music_VCA.setVolume(thisSlider.value);
        }
    }
    private void ChangeVolumeSfx()
    {
        if (!soundManager.sfx_VCA.isValid())
        {
            Debug.LogWarning("VCA is not valid! Check FMOD path.");
        }
        else
        {
            soundManager.sfx_VCA.setVolume(thisSlider.value);
        }
    }
}
