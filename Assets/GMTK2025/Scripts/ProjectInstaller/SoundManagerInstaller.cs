using Reflex.Core;
using UnityEngine;

public class SoundManagerInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private SoundManagerConfig soundManagerConfig;

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton<SoundManagerConfig>(soundManagerConfig);
    }
}
