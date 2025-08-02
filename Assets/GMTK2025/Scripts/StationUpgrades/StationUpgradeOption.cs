using PrimeTween;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class StationUpgradeOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private int optionIndex;

    [Space]
    [SerializeField]
    private string upgradeName;
    [TextArea]
    [SerializeField]
    private string upgradeDescription;

    [Space]
    [SerializeField]
    private StationUpgradesScreen screen;

    [SerializeField]
    private Transform statsPoint;

    [Space]
    [SerializeField]
    private GameObject highlightBackground;

    [SerializeField]
    private TransformFollower statsPanel;

    [SerializeField]
    private Animator orbitAnim;

    [Header("Visual")]
    [SerializeField]
    private Vector2 onHoverScale;

    [Tooltip("Time for the hover to occur")]
    [SerializeField]
    private float hoverAnimTime;

    [SerializeField]
    private float orbitAnimHoverSpeed;

    [Inject]
    private UpgradeManager upgradeManager;

    [Inject]
    private SoundManager soundManager;

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
            switch (optionIndex)
            {
                // Orbit upgrade
                case 0:
                    if (upgradeManager.OrbitUpgradeAmount >= 5)
                    {
                        // TODO: There needs to be a better system for when this happens
                        screen.HideScreen();
                        return;
                    }

                    upgradeManager.AddUpgrade(true);
                    soundManager.PlayOneShot(soundManager.soundManagerConfig.sfxUpgradePath);
                    screen.BuyOrbit();
                    break;
                // Invalid upgrade
                default:
                    upgradeManager.AddUpgrade();
                    soundManager.PlayOneShot(soundManager.soundManagerConfig.sfxUpgradePath);
                    return;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!upgradeManager.OnScreen)
        {
            HideOption();
        }

        mouseOver = true;

        // Visual
        Tween.Scale(transform, endValue: onHoverScale, hoverAnimTime);

        orbitAnim.speed = orbitAnimHoverSpeed;

        highlightBackground.SetActive(true);
        statsPanel.gameObject.SetActive(true);
        statsPanel.transform.position = statsPoint.position;
        statsPanel.FollowTarget = statsPoint;

        screen.UpdateStatsPanel(upgradeName, upgradeDescription);
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

        orbitAnim.speed = 1;

        highlightBackground.SetActive(false);
        statsPanel.gameObject.SetActive(false);
    }
}
