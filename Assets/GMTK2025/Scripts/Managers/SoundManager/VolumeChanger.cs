using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeChanger : MonoBehaviour
{
    [Inject]
    private SoundManager soundManager;
    [SerializeField]
    private VolumeType volumeType;

    [SerializeField]
    private UnityEngine.UI.Slider thisSlider;
    [SerializeField]
    private RadialSlider thisOtherSlider;
    private void Start()
    {
        if (thisSlider)
        {
            soundManager.PreResetVolume(volumeType);
        }
        else if (thisOtherSlider)
        {
            soundManager.PreResetVolume(volumeType);
        }
        ResetVolume();
    }
    private void OnEnable()
    {
        if (thisSlider)
        {
            soundManager.PreResetVolume(volumeType);
        }
        else if (thisOtherSlider)
        {
            soundManager.PreResetVolume(volumeType);
        }
        ResetVolume();
    }
    private void ResetVolume()
    {
        if (thisSlider)
        {
            switch (volumeType)
            {
                case VolumeType.None:
                    return;
                case VolumeType.Music:
                    thisSlider.value = soundManager.fVolumeMusic;
                    break;
                case VolumeType.Sfx:
                    thisSlider.value = soundManager.fVolumeSfx;
                    break;
            }
        }
        else if (thisOtherSlider)
        {
            switch (volumeType)
            {
                case VolumeType.None:
                    return;
                case VolumeType.Music:
                    thisOtherSlider.value = soundManager.fVolumeMusic;
                    break;
                case VolumeType.Sfx:
                    thisOtherSlider.value = soundManager.fVolumeSfx;
                    break;
            }
        }
    }
    public void VolumeChange()
    {
        if (thisSlider)
        {
            soundManager.ChangeVolume(volumeType, thisSlider.value);
        }
        else if (thisOtherSlider)
        {
            soundManager.ChangeVolume(volumeType, thisOtherSlider.fValue);
        }
    }
}
