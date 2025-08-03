using UnityEngine.Events;

public class FinanceManager
{
    public FinanceController FinanceController;

    public UnityEvent<int> OnProfitGenerated = new();

    private int money;
    public int Money
    {
        set
        {
            money = value;
            FinanceController.MoneyText.text = money + "$";
        }
        get
        {
            return money;
        }
    }

    public void GenerateShipProfit(int profit)
    {
        Money = money + profit;
        OnProfitGenerated.Invoke(profit);
    }

    public bool UseMoney(int amount)
    {
        if (money < amount)
        {
            return false;
        }

        Money = money - amount;
        return true;
    }
}
