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

    [SerializeField]
    private GameObject disabledVisual;

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

    private bool disabled;

    // Upgrade data info
    private UpgradeData data;

    private UpgradeType upgradeType;
    private string upgradeName;
    private string upgradeDescription;
    private int upgradeCost;

    // Initial
    private Vector2 defaultScale;

    private InputAction interactAction;

    void Awake()
    {
        if (startUpgradeData != null)
        {
            SetUpgradeOption(startUpgradeData);
        }
    }

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
            if (BuyUpgrade())
            {
                soundManager.PlayOneShot(soundManager.soundManagerConfig.sfxUpgradePath);
            }
        }
    }

    private bool BuyUpgrade()
    {
        if (!financeManager.UseMoney(upgradeCost) || disabled)
        {
            return false;
        }

        switch (upgradeType)
        {
            case UpgradeType.Orbit:
                screen.BuyOrbit();
                break;
            case UpgradeType.Life:
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
        if (disabled)
        {
            return;
        }

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
        data = upgradeData;

        upgradeType = data.UpgradeType;

        UpdateData();
    }

    public void UpdateData()
    {
        int upgradeAmount = upgradeManager.UpgradeAmount[(int)upgradeType];

        upgradeName = data.Name[UtilityFunctions.GetKeyOrLast(upgradeAmount, data.Name.Length)];
        upgradeDescription = data.Descriptions[UtilityFunctions.GetKeyOrLast(upgradeAmount, data.Descriptions.Length)];
        upgradeIcon.sprite = data.Icons[UtilityFunctions.GetKeyOrLast(upgradeAmount, data.Icons.Length)];
        upgradeCost = data.Cost[UtilityFunctions.GetKeyOrLast(upgradeAmount, data.Cost.Length)];
        upgradeCost += upgradeManager.Config.UpgradeCostPerPurchase[upgradeManager.TotalUpgradeAmount];

        costText.text = upgradeCost + "$";
    }

    public void DisableUpgradeOption()
    {
        disabled = true;
        disabledVisual.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideOption();
    }

    private void OnEnable()
    {
        UpdateData();  
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
