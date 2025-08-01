using Reflex.Attributes;
using UnityEngine;

public class FinanceManager
{
    public FinanceController FinanceController;

    private float money;
    public float Money
    {
        set
        {
            money = value;
            FinanceController.MoneyText.text = Mathf.Floor(money) + "$";
        }
        get
        {
            return Mathf.Floor(money);
        }
    }

    public void GenerateShipProfit(float profit)
    {
        Money = money + (profit * 0.001f);
    }
}
