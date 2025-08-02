using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class OrbitUpgradeOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Space]
    [SerializeField]
    private GameObject highlightBackground;

    [SerializeField]
    private GameObject statsPanel;

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
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;

        // Visual
        Tween.Scale(transform, endValue: onHoverScale, hoverAnimTime);

        highlightBackground.SetActive(true);
        statsPanel.SetActive(true);
        statsPanel.transform.position = transform.position + new Vector3(0, statsYOffset);

        // TODO: screen.UpdateStatsPanel(optionIndex);
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

        // Visual
        Tween.Scale(transform, endValue: defaultScale, hoverAnimTime);

        highlightBackground.SetActive(false);
        statsPanel.SetActive(false);
    }
}
