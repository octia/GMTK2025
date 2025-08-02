using Reflex.Core;
using UnityEngine;

public class ConfigInstaller : MonoBehaviour, IInstaller
{
    [SerializeField]
    private SceneManagerConfig sceneManagerConfig;

    [SerializeField]
    private CameraControlConfig cameraControlConfig;

    [SerializeField]
    private TimerConfig timerConfig;

    [SerializeField]
    private ShipCollisionManagerConfig shipCollisionManagerConfig;

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton(sceneManagerConfig);
        builder.AddSingleton(cameraControlConfig);
        builder.AddSingleton(timerConfig);
        builder.AddSingleton(shipCollisionManagerConfig);
    }
}
