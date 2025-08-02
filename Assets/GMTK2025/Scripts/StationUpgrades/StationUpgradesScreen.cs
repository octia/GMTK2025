using System.Collections.Generic;
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

    [Inject]
    private UpgradeManager upgradeManager;

    [Inject]
    private FinanceManager financeManager;

    [Inject]
    private OrbitController orbitController;

    private void Awake()
    {
        upgradeManager.StationUpgradesScreen = this;
    }

    private void OnDisable()
    {
        mainScreen.SetActive(false);
    }

    public void BuyButton()
    {
        if (
            !financeManager.UseMoney(
                upgradeManager.Config.UpgradeCostPerPurchase[upgradeManager.UpgradeAmount]
            )
        )
        {
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

        int n = 3;
        foreach (var orbit in orbitController.GetLockedOrbits())
        {
            orbit.State = Orbit.VisualState.Alert;

            n--;
            if (n == 0)
            {
                break;
            }
        }
    }

    public void HideScreen()
    {
        mainScreen.SetActive(false);
        upgradeManager.OnScreen = false;

        buyButton.SetActive(true);
    }
}
