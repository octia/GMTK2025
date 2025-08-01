using Reflex.Core;
using UnityEngine;

public class OrbitControllerInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] OrbitController controller;

    public void InstallBindings(ContainerBuilder containerBuilder)
    {
        containerBuilder.AddSingleton(controller);
    }
}
