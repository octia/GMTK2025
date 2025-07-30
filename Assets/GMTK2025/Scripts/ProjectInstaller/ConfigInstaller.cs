using Reflex.Core;
using UnityEngine;

public class ConfigInstaller : MonoBehaviour, IInstaller
{
    [SerializeField]
    private SceneManagerConfig sceneManagerConfig;

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton<SceneManagerConfig>(sceneManagerConfig);
    }
}
