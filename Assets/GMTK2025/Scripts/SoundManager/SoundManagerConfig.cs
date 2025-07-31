using UnityEngine;

[CreateAssetMenu(menuName = "GMTK2025/Config/SoundManagerConfig", fileName = "SoundManagerConfig")]
public class SoundManagerConfig : ScriptableObject
{
    [field: SerializeField]
    public FMOD.Studio.VCA music_VCA;
    [field: SerializeField]
    public FMOD.Studio.VCA sfx_VCA;
}
