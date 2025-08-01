using Reflex.Attributes;
using TMPro;
using UnityEngine;

public class FinanceController : MonoBehaviour
{
    public TMP_Text MoneyText;

    [Inject]
    private FinanceManager financeManager;

    private void Awake()
    {
        financeManager.FinanceController = this;
        financeManager.Money = 0;
    }
}
