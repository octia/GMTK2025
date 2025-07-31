using Reflex.Core;
using UnityEngine;

public class PlayerManagerInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private PlayerManagerConfig playerManagerConfig;

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton<PlayerManagerConfig>(playerManagerConfig);
    }
}
