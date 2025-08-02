using System.Linq;
using PrimeTween;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class OrbitUpgradeOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private int optionIndex;

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

    [Inject]
    private OrbitController orbitController;

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
            
            for (int i = 0; i < 3; i++)
            {
                if (i == optionIndex)
                {
                    orbitController.UnlockOrbit(screen.NewOrbits[i]);
                    screen.NewOrbits[i].State = Orbit.VisualState.Visible;
                    continue;
                }

                screen.NewOrbits[i].State = Orbit.VisualState.Invisible;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;

        // TODO: Show preview of the orbit

        screen.NewOrbits[optionIndex].State = Orbit.VisualState.Hightlighted;

        // Visual
        Tween.Scale(transform, endValue: onHoverScale, hoverAnimTime);

        //statsPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        for (int i = 0; i < 3; i++)
        {
            screen.NewOrbits[i].State = Orbit.VisualState.Alert;
        }

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
