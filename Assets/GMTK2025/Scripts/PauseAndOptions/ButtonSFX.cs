using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using FMODUnity;
using Reflex.Attributes;

[RequireComponent(typeof(Button))]
public class ButtonSFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Inject]
    private SoundManager soundManager;
    public void OnPointerEnter(PointerEventData eventData)
    {
        soundManager.PlaySfxUiPointer();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        soundManager.PlaySfxUiClick();
    }
}
