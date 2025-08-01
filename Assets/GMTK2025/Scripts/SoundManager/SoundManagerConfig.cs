using UnityEngine;

[CreateAssetMenu(fileName = "SoundManagerConfig", menuName = "Scriptable Objects/SoundManagerConfig")]
public class SoundManagerConfig : ScriptableObject
{
    [SerializeField]
    public string musicBusName = "bus:/Music";

    [SerializeField]
    public string vfxBusName = "bus:/SFX";

    [SerializeField]
    public string sfxUiClickPath = "path";

    [SerializeField]
    public string sfxUiPointerPath = "path";

    public FMOD.Studio.EventInstance musicLoopInstance;
}
