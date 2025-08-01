using System;
using Reflex.Attributes;

public class LifeManager : IDisposable
{
    public Action OnLifeCountChanged;

    [Inject]
    private LifeConfig config;

    [Inject]
    private SceneManager sceneManager;

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
            sceneManager.LoadGameplay();
        }
    }

    public void GainLife(int livesToGain = 1)
    {
        LifeCount += livesToGain;
        OnLifeCountChanged?.Invoke();
    }

    public void Dispose() { }
}
