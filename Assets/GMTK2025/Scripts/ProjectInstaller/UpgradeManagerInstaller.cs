using Reflex.Core;
using UnityEngine;

public class UpgradeManagerInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private UpgradeManagerConfig upgradeManagerConfig;

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton<UpgradeManager>();
        builder.AddSingleton<UpgradeManagerConfig>(upgradeManagerConfig);
    }
}
