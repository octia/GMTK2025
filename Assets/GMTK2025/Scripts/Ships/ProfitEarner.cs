using System.Collections;
using Reflex.Attributes;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class ProfitEarner : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem profitParticles;

    [SerializeField]
    private ParticleSystem sadParticles;

    [Inject]
    private TimerManager timerManager;

    [Inject]
    private FinanceManager financeManager;

    private Ship ship;

    private float profitEarnedSoFar = 0;

    public bool NotGeneratingMoney => !ship.enabled
            || ship.PreferredOrbit == null
            || ship.Orbit != ship.PreferredOrbit
            || ship.DistanceToOrbit > 1.0f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(Random.value);
        while (true)
        {
            if (NotGeneratingMoney && timerManager.TimeScale > 0.1f)
            {
                sadParticles.Emit(1);
            }
            yield return new WaitForSeconds(2.0f / ship.ShipData.Profit);
        }
    }

    private void Awake()
    {
        ship = GetComponent<Ship>();
    }

    private void FixedUpdate()
    {
        if (NotGeneratingMoney)
        {
            return;
        }

        float profitThisStep =
            Time.fixedDeltaTime * timerManager.TimeScale * ship.ShipData.Profit * 0.2f;

        if ((int)profitEarnedSoFar / 1 != (int)(profitEarnedSoFar + profitThisStep) / 1)
        {
            financeManager.GenerateShipProfit(1);
        }
        if ((int)(profitEarnedSoFar * 4) / 1 != (int)((profitEarnedSoFar + profitThisStep) * 4) / 1)
        {
            profitParticles.Emit(1);
        }
        profitEarnedSoFar += profitThisStep;
    }
}
