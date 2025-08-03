using System;
using Reflex.Attributes;
using UnityEngine;

public class LifeManager : IDisposable
{
    [Inject]
    public LifeConfig Config;

    [Inject]
    private GameOverController gameOverController;

    public LifeController LifeController;

    public int LifeCount { get; private set; } = 0;
    public int MaxLifeCount;

    public void LoseLife()
    {
        LifeCount--;
        LifeController.VisualLifeChange();
        if (LifeCount <= 0)
        {
            gameOverController.GameOver();
        }
    }

    public void GainLife(int livesToGain = 1)
    {
        LifeCount += Mathf.Clamp(livesToGain, 0, MaxLifeCount);

        LifeController.VisualLifeChange();
    }

    public void GainMaxLife(int amount = 1)
    {
        MaxLifeCount += amount;

        LifeController.AddHeartImages(amount);
        Debug.Log("Adding hearts! " + amount);

        GainLife(amount);
    }

    public void Dispose() { }
}
