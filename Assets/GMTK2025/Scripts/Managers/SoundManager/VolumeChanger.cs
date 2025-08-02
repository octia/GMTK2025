using Reflex.Attributes;
using UnityEngine;

public class VolumeChanger : MonoBehaviour
{
    [Inject]
    private SoundManager soundManager;
    [SerializeField]
    private VolumeType volumeType;

    private UnityEngine.UI.Slider thisSlider;
    private void Start()
    {
        thisSlider = transform.GetComponent<UnityEngine.UI.Slider>();
        soundManager.ResetVolume(volumeType, thisSlider);
    }
    private void OnEnable()
    {
        soundManager.ResetVolume(volumeType, thisSlider);
    }
    public void VolumeChange()
    {
        soundManager.ChangeVolume(volumeType, thisSlider.value);
    }
}
