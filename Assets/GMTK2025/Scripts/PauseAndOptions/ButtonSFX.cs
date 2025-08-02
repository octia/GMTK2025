using Reflex.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Inject]
    private SoundManager soundManager;

    public void OnPointerEnter(PointerEventData eventData)
    {
        soundManager.PlayOneShot(soundManager.soundManagerConfig.sfxUiPointerPath);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        soundManager.PlayOneShot(soundManager.soundManagerConfig.sfxUiClickPath);
    }
}
