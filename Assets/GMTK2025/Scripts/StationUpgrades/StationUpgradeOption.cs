using PrimeTween;
using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class StationUpgradeOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private UpgradeType upgrade;

    [Space]
    [SerializeField]
    private string upgradeName;

    [TextArea]
    [SerializeField]
    private string upgradeDescription;

    [SerializeField]
    private int upgradeCost;

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
    private TMP_Text costText;

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

    [Inject]
    private LifeManager lifeManager;

    [Inject]
    private FinanceManager financeManager;

    private bool mouseOver;

    // Initial
    private Vector2 defaultScale;

    private InputAction interactAction;

    private void Start()
    {
        costText.text = upgradeCost == 0 ? "Free!" : upgradeCost + "$";

        // Grabbing initial variables
        defaultScale = transform.localScale;

        interactAction = InputSystem.actions.FindAction("Click");
    }

    private void Update()
    {
        if (mouseOver && interactAction.WasPressedThisFrame())
        {
            if (BuyUpgrade())
            {
                soundManager.PlayOneShot(soundManager.soundManagerConfig.sfxUpgradePath);
            }
        }
    }

    private bool BuyUpgrade()
    {
        switch (upgrade)
        {
            case UpgradeType.Orbit:
                if (upgradeManager.UpgradeAmount[(int)UpgradeType.Orbit] >= 5)
                {
                    // TODO: There needs to be a better system for when this happens
                    screen.HideScreen();
                    return false;
                }

                screen.BuyOrbit();
                break;
            case UpgradeType.Life:
                if (!financeManager.UseMoney(upgradeCost))
                {
                    return false;
                }

                lifeManager.GainMaxLife();
                break;
            case UpgradeType.Time:
                screen.BuyTimeSlow();
                break;
        }

        if (upgrade != UpgradeType.Orbit)
        {
            screen.HideScreen();
        }

        upgradeManager.AddUpgrade(upgrade);
        return true;
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
