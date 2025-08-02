using Reflex.Attributes;
using UnityEngine;

public class FinanceManager
{
    public FinanceController FinanceController;

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
    }
}
