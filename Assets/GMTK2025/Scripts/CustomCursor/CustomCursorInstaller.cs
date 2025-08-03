using Reflex.Core;
using UnityEngine;

public class CustomCursorInstaller : MonoBehaviour, IInstaller
{
    [SerializeField]
    private CustomCursor controller;

    public void InstallBindings(ContainerBuilder containerBuilder)
    {
        containerBuilder.AddSingleton(controller);
    }
}
