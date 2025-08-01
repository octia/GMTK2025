using Reflex.Core;
using UnityEngine;

public class ProjectInstaller : MonoBehaviour, IInstaller
{
    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton<SceneManager>();
        builder.AddSingleton<SoundManager>();
        builder.AddSingleton<TimerManager>();
        builder.AddSingleton<ShipCollisionManager>();
        builder.AddSingleton<FinanceManager>();
    }
}
