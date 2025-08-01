using Reflex.Core;
using UnityEngine;

public class LifeInstaller : MonoBehaviour, IInstaller
{
    [SerializeField]
    private LifeConfig config;

    public void InstallBindings(ContainerBuilder containerBuilder)
    {
        containerBuilder.AddSingleton(config);
        containerBuilder.AddSingleton<LifeManager>();
    }
}
