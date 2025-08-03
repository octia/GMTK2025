using Reflex.Attributes;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [Inject]
    private TimerManager timerManager;

    int totalGold;

    float runTime;

    private void Update()
    {
        if (timerManager.IsPaused)
        {
            return;
        }

        runTime += Time.deltaTime;
    }
}