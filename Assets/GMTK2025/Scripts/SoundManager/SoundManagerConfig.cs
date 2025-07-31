using UnityEngine;

[CreateAssetMenu(fileName = "SoundManagerConfig", menuName = "Scriptable Objects/SoundManagerConfig")]
public class SoundManagerConfig : ScriptableObject
{
    [SerializeField]
    public string musicBusName = "vca:/Music";

    [SerializeField]
    public string vfxBusName = "vca:/SFX";
}
