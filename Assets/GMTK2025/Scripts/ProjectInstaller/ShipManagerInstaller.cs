using Reflex.Core;
using UnityEngine;

public class ShipManagerInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private ShipManagerConfig shipManagerConfig;

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton<ShipManagerConfig>(shipManagerConfig);
    }
}
