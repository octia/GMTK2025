using System.Collections.Generic;
using System.Linq;
using Reflex.Attributes;
using TMPro;
using UnityEngine;

public class StationUpgradesScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject mainScreen;

    public TMP_Text CostText;

    [SerializeField]
    private GameObject buyButton;


    [SerializeField]
    private GameObject timeSlowAbility;


    [Space]
    [SerializeField]
    private GameObject upgradeOptions;

    [SerializeField]
    private GameObject orbitOptions;


    [Space]
    [SerializeField]
    private TMP_Text statsDescriptionText;

    [SerializeField]
    private TMP_Text statsNameText;


    // Managers

    [Inject]
    private UpgradeManager upgradeManager;

    [Inject]
    private FinanceManager financeManager;

    [Inject]
    private OrbitController orbitController;

    [Inject]
    private SoundManager soundManager;


    [HideInInspector]
    public Orbit[] NewOrbits;

    private void Awake()
    {
        upgradeManager.StationUpgradesScreen = this;
        CostText.text = upgradeManager.Config.UpgradeCostPerPurchase[0] + "$";
    }

    private void OnDisable()
    {
        mainScreen.SetActive(false);
    }

    public void BuyButton()
    {
        if (
            !financeManager.UseMoney(
                upgradeManager.Config.UpgradeCostPerPurchase[upgradeManager.TotalUpgradeAmount]
            )
        )
        {
            soundManager.PlayOneShot(soundManager.soundManagerConfig.sfxErrorPath);
            return;
        }

        mainScreen.SetActive(true);
        upgradeOptions.SetActive(true);
        orbitOptions.SetActive(false);

        upgradeManager.OnScreen = true;

        buyButton.SetActive(false);
    }

    public void UpdateStatsPanel(string name, string description)
    {
        statsNameText.text = name;
        statsDescriptionText.text = description;
    }

    public void BuyOrbit()
    {
        upgradeOptions.SetActive(false);
        orbitOptions.SetActive(true);

        // TODO: Shuffle the orbits

        NewOrbits = new Orbit[3];

        List<Orbit> lockedOrbits = orbitController.GetLockedOrbits().ToList();
        ShipManager.Shuffle(lockedOrbits);

        int n = 0;
        foreach (var orbit in lockedOrbits)
        {
            orbit.State = Orbit.VisualState.Alert;

            NewOrbits[n] = orbit;

            n++;
            if (n == 3)
            {
                break;
            }
        }
    }

    public void BuyTimeSlow()
    {
        if (upgradeManager.UpgradeAmount[(int)UpgradeType.Time] == 0)
        {
            timeSlowAbility.SetActive(true);
            return;
        }

        // TODO: Implement what happens on more time slow ability
        Debug.LogWarning("Further time slow is not implemented yet!");
    }

    public void HideScreen()
    {
        mainScreen.SetActive(false);
        upgradeManager.OnScreen = false;

        buyButton.SetActive(true);
    }
}
