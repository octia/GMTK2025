using PrimeTween;
using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StationUpgradeOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("Starts the object with a upgrade data.\n Can be overwritten by the screen.")]
    [SerializeField]
    private UpgradeData startUpgradeData;

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
    private Image upgradeIcon;

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

    // Upgrade data info
    private UpgradeType upgradeType;
    private string upgradeName;
    private string upgradeDescription;
    private int upgradeCost;

    private bool mouseOver;

    // Initial
    private Vector2 defaultScale;

    private InputAction interactAction;

    private void Start()
    {
        if (startUpgradeData != null)
        {
            SetUpgradeOption(startUpgradeData);
        }

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
        switch (upgradeType)
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
            case UpgradeType.Direction:
                screen.BuyDirectionSwitcher();
                break;
        }

        if (upgradeType != UpgradeType.Orbit)
        {
            screen.HideScreen();
        }

        upgradeManager.AddUpgrade(upgradeType);
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

    public void SetUpgradeOption(UpgradeData upgradeData)
    {
        upgradeType = upgradeData.UpgradeType;

        int upgradeAmount = upgradeManager.UpgradeAmount[(int)upgradeType];

        upgradeName = upgradeData.Name[UtilityFunctions.GetKeyOrLast(upgradeAmount, upgradeData.Name.Length)];
        upgradeDescription = upgradeData.Descriptions[UtilityFunctions.GetKeyOrLast(upgradeAmount, upgradeData.Descriptions.Length)];
        upgradeCost = upgradeData.Cost[UtilityFunctions.GetKeyOrLast(upgradeAmount, upgradeData.Cost.Length)];
        upgradeIcon.sprite = upgradeData.Icons[UtilityFunctions.GetKeyOrLast(upgradeAmount, upgradeData.Icons.Length)];

        costText.text = upgradeCost == 0 ? "Free!" : upgradeCost + "$";
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
