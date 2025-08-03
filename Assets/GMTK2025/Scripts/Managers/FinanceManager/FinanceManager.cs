using Reflex.Attributes;
using UnityEngine.Events;

public class FinanceManager
{
    public FinanceController FinanceController;

    public UnityEvent<int> OnProfitGenerated = new();

    [Inject]
    private PlayerDialogueController playerDialogue;

    private bool hasPurchasedUpgradeOrSeenTutorial = false;

    private int money;
    public int Money
    {
        set
        {
            money = value;
            FinanceController.MoneyText.text = money + "$";
        }
        get { return money; }
    }

    public void GenerateShipProfit(int profit)
    {
        Money = money + profit;
        OnProfitGenerated.Invoke(profit);

        if (Money > 55 && !hasPurchasedUpgradeOrSeenTutorial)
        {
            hasPurchasedUpgradeOrSeenTutorial = true;
            playerDialogue.PlayTextChain(PlayerDialogueChainType.PurchaseUpgradeTutorial);
        }
    }

    public bool UseMoney(int amount)
    {
        if (money < amount)
        {
            return false;
        }

        hasPurchasedUpgradeOrSeenTutorial = true;
        Money = money - amount;
        return true;
    }
}
