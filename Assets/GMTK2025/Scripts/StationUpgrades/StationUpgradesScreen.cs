using Reflex.Attributes;
using TMPro;
using UnityEngine;

public class StationUpgradesScreen : MonoBehaviour
{
    public bool OnScreen;

    [Space]
    [SerializeField]
    private GameObject mainScreen;

    public TMP_Text CostText;

    [SerializeField]
    private GameObject buyButton;

    [Inject]
    private UpgradeManager upgradeManager;

    [Inject]
    private FinanceManager financeManager;

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
        OnScreen = true;

        buyButton.SetActive(false);
    }

    public void UpdateStatsPanel(int i)
    {
        // TODO: Update stats panel depending on the upgrade that is being hovered over
    }

    public void HideScreen()
    {
        mainScreen.SetActive(false);
        OnScreen = false;

        buyButton.SetActive(true);
    }
}
