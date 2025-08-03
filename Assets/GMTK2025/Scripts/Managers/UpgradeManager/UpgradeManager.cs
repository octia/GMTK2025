using System;
using Reflex.Attributes;

public class UpgradeManager
{
    [Inject]
    public UpgradeManagerConfig Config;

    public StationUpgradesScreen StationUpgradesScreen;

    public bool OnScreen;

    public int[] UpgradeAmount;

    private int totalUpgradeAmount;
    public int TotalUpgradeAmount
    {
        set
        {
            totalUpgradeAmount = value;
            StationUpgradesScreen.CostText.text =
                "(" + Config.UpgradeCostPerPurchase[TotalUpgradeAmount] + "$)";
        }
        get { return totalUpgradeAmount; }
    }

    [Inject]
    public void Initialize()
    {
        UpgradeAmount = new int[Enum.GetNames(typeof(UpgradeType)).Length];
    }

    public void AddUpgrade(UpgradeType upgrade)
    {
        TotalUpgradeAmount = totalUpgradeAmount + 1;

        UpgradeAmount[(int)upgrade]++;
    }
}
