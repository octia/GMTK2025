using Reflex.Core;
using UnityEngine;

public class GameplayInstaller : MonoBehaviour, IInstaller
{
    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton<TimerManager>();
        builder.AddSingleton<ShipManager>();
        builder.AddSingleton<ShipCollisionManager>();
        builder.AddSingleton<FinanceManager>();
    }
}
