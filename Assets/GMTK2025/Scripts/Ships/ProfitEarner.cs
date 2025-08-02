using Reflex.Attributes;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class ProfitEarner : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem profitParticles;

    [Inject]
    private TimerManager timerManager;

    [Inject]
    private FinanceManager financeManager;

    private Ship ship;

    private float profitEarnedSoFar = 0;

    private void Awake()
    {
        ship = GetComponent<Ship>();
    }

    private void FixedUpdate()
    {
        if (
            !ship.enabled
            || ship.PreferredOrbit == null
            || ship.Orbit != ship.PreferredOrbit
            || ship.DistanceToOrbit > 1.0f
        )
        {
            return;
        }

        float profitThisStep =
            Time.fixedDeltaTime * timerManager.TimeScale * ship.ShipData.Profit * 0.2f;

        if ((int)profitEarnedSoFar / 1 != (int)(profitEarnedSoFar + profitThisStep) / 1)
        {
            profitParticles.Emit(1);
            financeManager.GenerateShipProfit(1);
        }
        profitEarnedSoFar += profitThisStep;
    }
}
