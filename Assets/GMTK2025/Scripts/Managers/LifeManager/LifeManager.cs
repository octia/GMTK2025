using System;
using Reflex.Attributes;
using UnityEngine;

public class LifeManager : IDisposable
{
    public Action OnLifeCountChanged;

    [Inject]
    public LifeConfig config;

    [Inject]
    private SceneManager sceneManager;

    [Inject]
    private GameOverController gameOverController;

    public int LifeCount { get; private set; } = 3;

    [Inject]
    private void Initialize()
    {
        LifeCount = config.StartLifeCount;
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
        LifeCount += livesToGain;
        OnLifeCountChanged?.Invoke();
    }

    public void Dispose() { }
}
