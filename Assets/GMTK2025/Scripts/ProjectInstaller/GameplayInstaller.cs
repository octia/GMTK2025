using Reflex.Core;
using UnityEngine;

public class GameplayInstaller : MonoBehaviour, IInstaller
{
    [SerializeField]
    private GameOverController gameOverController;

    [SerializeField]
    private CameraController mainCamera;

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton<TimerManager>();
        builder.AddSingleton<ShipCollisionManager>();
        builder.AddSingleton<FinanceManager>();
        builder.AddSingleton<UIManager>();
        builder.AddSingleton(gameOverController);
        builder.AddSingleton(mainCamera);
    }
}
