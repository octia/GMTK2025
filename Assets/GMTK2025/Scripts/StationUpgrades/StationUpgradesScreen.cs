using System;
using System.Collections.Generic;
using System.Linq;
using Reflex.Attributes;
using TMPro;
using UnityEngine;

public class StationUpgradesScreen : MonoBehaviour
{
    [Space]
    [SerializeField]
    private List<UpgradeData> upgradePool;

    [SerializeField]
    private GameObject mainScreen;

    public TMP_Text CostText;

    [SerializeField]
    private GameObject buyButton;


    [SerializeField]
    private GameObject timeSlowAbility;

    [SerializeField]
    private GameObject directionSwitcherAbility;


    [Space]
    [SerializeField]
    private GameObject upgradeOptionsContainer;

    [SerializeField]
    private GameObject orbitUpgradeOption;

    [SerializeField]
    private StationUpgradeOption[] randomUpgradeOptions;

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
        upgradeOptionsContainer.SetActive(true);
        orbitOptions.SetActive(false);

        upgradeManager.OnScreen = true;

        buyButton.SetActive(false);

        // In case player picks 5 orbits it doesnt show this options anymore
        if (upgradeManager.UpgradeAmount[(int)UpgradeType.Orbit] >= 5)
        {
            orbitUpgradeOption.gameObject.SetActive(false);
            randomUpgradeOptions[2].gameObject.SetActive(true);
        }

        // Shuffling upgrades to buttons
        UtilityFunctions.Shuffle(upgradePool);

        for (int i = 0; i < 3; i++)
        {
            randomUpgradeOptions[i].SetUpgradeOption(upgradePool[i]);
        }
    }

    public void UpdateStatsPanel(string name, string description)
    {
        statsNameText.text = name;
        statsDescriptionText.text = description;
    }

    public void BuyOrbit()
    {
        upgradeOptionsContainer.SetActive(false);
        orbitOptions.SetActive(true);

        // TODO: Shuffle the orbits

        NewOrbits = new Orbit[3];

        List<Orbit> lockedOrbits = orbitController.GetLockedOrbits().ToList();
        UtilityFunctions.Shuffle(lockedOrbits);

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
        Debug.LogWarning("Further time slow upgrade is not implemented yet!");
    }

    public void BuyDirectionSwitcher()
    {
        if (upgradeManager.UpgradeAmount[(int)UpgradeType.Direction] == 0)
        {
            directionSwitcherAbility.SetActive(true);
            return;
        }

        // TODO: Implement what happens on more time slow ability
        Debug.LogWarning("Further direction switching upgrade is not implemented yet!");
    }

    public void HideScreen()
    {
        mainScreen.SetActive(false);
        upgradeManager.OnScreen = false;

        buyButton.SetActive(true);
    }
}
