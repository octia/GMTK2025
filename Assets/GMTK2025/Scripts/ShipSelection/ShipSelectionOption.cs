using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShipSelectionOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int optionIndex;

    [Space]
    [SerializeField] private ShipSelectionScreen screen;
    [SerializeField] private Transform statsPoint;

    [Space]
    [SerializeField] private GameObject highlightBackground;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private Animator orbitAnim;

    [Space]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;

    [Header("Visual")]
    [SerializeField] private Vector2 onHoverScale;
    [Tooltip("Time for the hover to occur")]
    [SerializeField] private float hoverAnimTime;
    [SerializeField] private float orbitAnimHoverSpeed;

    private bool mouseOver;

    // Initial
    private Vector2 defaultScale;

    private InputAction interactAction;

    private void Start()
    {
        // Grabbing initial variables
        defaultScale = transform.localScale;

        interactAction = InputSystem.actions.FindAction("Click");
    }

    private void Update()
    {
        if (mouseOver && interactAction.WasPressedThisFrame())
        {
            screen.SelectNewShip(optionIndex);
        }
    }

    public void ChangeSelectionShip(ShipData ship)
    {
        icon.sprite = ship.Icon;
        nameText.text = ship.name;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!screen.OnScreen)
        {
            HideOption();
        }

        mouseOver = true;

        // Visual        
        Tween.Scale(transform, endValue: onHoverScale, hoverAnimTime);

        orbitAnim.speed = orbitAnimHoverSpeed;

        highlightBackground.SetActive(true);
        statsPanel.SetActive(true);
        statsPanel.transform.position = statsPoint.position;
        statsPanel.GetComponent<TransformFollower>().FollowTarget = statsPoint;

        screen.UpdateStatsPanel(optionIndex);
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

        screen.ResetDialogPanel();

        orbitAnim.speed = 1;

        highlightBackground.SetActive(false);
        statsPanel.SetActive(false);
    }
}
