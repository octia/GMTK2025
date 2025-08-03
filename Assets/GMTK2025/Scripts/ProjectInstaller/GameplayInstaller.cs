using Reflex.Core;
using UnityEngine;

public class GameplayInstaller : MonoBehaviour, IInstaller
{
    [SerializeField]
    private CustomCursor customCursor;

    [SerializeField]
    private OrbitController orbitController;

    [SerializeField]
    private ScoreController scoreController;

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
        
        builder.AddSingleton(customCursor);
        builder.AddSingleton(orbitController);
        //builder.AddSingleton(scoreController);
        builder.AddSingleton(gameOverController);

        builder.AddSingleton(mainCamera);
    }
}
