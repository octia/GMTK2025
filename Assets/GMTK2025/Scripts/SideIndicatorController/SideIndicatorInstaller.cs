using Reflex.Core;
using UnityEngine;

public class SideIndicatorInstaller : MonoBehaviour, IInstaller
{
    [SerializeField]
    private NewShipIndicatorController shipIndicatorController;

    public void InstallBindings(ContainerBuilder containerBuilder)
    {
        containerBuilder.AddSingleton(shipIndicatorController);
    }
}
