using Reflex.Core;
using UnityEngine;

public class ConfigInstaller : MonoBehaviour, IInstaller
{
    [SerializeField]
    private SceneManagerConfig sceneManagerConfig;

    [SerializeField]
    private CameraControlConfig cameraControlConfig;

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton(sceneManagerConfig);
        builder.AddSingleton(cameraControlConfig);
    }
}
