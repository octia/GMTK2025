using System;
using Reflex.Attributes;
using UnityEngine;

public class LifeManager : IDisposable
{
    public Action OnLifeCountChanged;

    [Inject]
    public LifeConfig Config;

    [Inject]
    private GameOverController gameOverController;

    public LifeController LifeController;

    public int LifeCount { get; private set; } = 2;
    public int MaxLifeCount;

    [Inject]
    private void Initialize()
    {
        LifeCount = Config.StartLifeCount;
        MaxLifeCount = Config.MaxLifeCount;
    }

    public void LoseLife()
    {
        LifeCount--;
        OnLifeCountChanged?.Invoke();
        if (LifeCount <= 0)
        {
            gameOverController.GameOver();
        }
    }

    public void GainLife(int livesToGain = 1)
    {
        LifeCount += Mathf.Clamp(livesToGain, 0, MaxLifeCount);
        OnLifeCountChanged?.Invoke();
    }

    public void GainMaxLife(int amount = 1)
    {
        MaxLifeCount += amount;

        LifeController.AddHeartImages(amount);

        GainLife(amount);
    }

    public void Dispose() { }
}
