using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ButtonSFX))]
public class MainMenuButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private UnityEvent onClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clickeeed");
        onClick.Invoke();
    }
}
