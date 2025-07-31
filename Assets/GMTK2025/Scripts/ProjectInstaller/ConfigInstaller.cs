using Reflex.Core;
using UnityEngine;

public class ConfigInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private SceneManagerConfig sceneManagerConfig;
    [SerializeField] private PlayerManagerConfig playerManagerConfig;

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton<SceneManagerConfig>(sceneManagerConfig);
        builder.AddSingleton<PlayerManagerConfig>(playerManagerConfig);
    }
}
