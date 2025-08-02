using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class OrbitUpgradeOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private StationUpgradesScreen screen;

    [SerializeField]
    private GameObject statsPanel;

    [SerializeField]
    [TextArea]
    private string description;

    [Header("Visual")]
    [SerializeField]
    private Vector2 onHoverScale;

    [Tooltip("Time for the hover to occur")]
    [SerializeField]
    private float hoverAnimTime;

    [SerializeField]
    private int statsYOffset;

    private bool mouseOver;

    // Initial
    private Vector2 defaultScale;

    private InputAction interactAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Grabbing initial variables
        defaultScale = transform.localScale;

        interactAction = InputSystem.actions.FindAction("Click");
    }

    // Update is called once per frame
    private void Update()
    {
        if (mouseOver && interactAction.WasPressedThisFrame())
        {
            // Add orbit
            screen.HideScreen();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;

        // TODO: Show preview of the orbit

        // Visual
        Tween.Scale(transform, endValue: onHoverScale, hoverAnimTime);

        //statsPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideOption();
    }

    private void OnDisable()
    {
        HideOption();
    }

    private void HideOption()
    {
        mouseOver = false;

        // TODO: Hide preview of orbit

        // Visual
        Tween.Scale(transform, endValue: defaultScale, hoverAnimTime);

        //statsPanel.SetActive(false);
    }
}
