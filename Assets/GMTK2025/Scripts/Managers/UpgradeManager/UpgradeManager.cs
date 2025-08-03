using Reflex.Attributes;

public class UpgradeManager
{
    [Inject]
    public UpgradeManagerConfig Config;

    public StationUpgradesScreen StationUpgradesScreen;

    public bool OnScreen;

    public int OrbitUpgradeAmount;

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

    public void AddUpgrade(bool orbitUpgrade = false)
    {
        UpgradeAmount = upgradeAmount + 1;

        if (orbitUpgrade)
        {
            OrbitUpgradeAmount++;
        }
    }
}
