using Reflex.Attributes;
using UnityEngine;

public class UpgradeManager
{
    [Inject]
    public UpgradeManagerConfig Config;

    public StationUpgradesScreen StationUpgradesScreen;

    private int upgradeAmount;
    public int UpgradeAmount
    {
        set
        {
            upgradeAmount = value;
            StationUpgradesScreen.CostText.text =
                "(" + Config.UpgradeCostPerPurchase[UpgradeAmount] + "$)";
        }
        get { return upgradeAmount; }
    }

    public void AddUpgrade()
    {
        UpgradeAmount = upgradeAmount + 1;
    }
}
