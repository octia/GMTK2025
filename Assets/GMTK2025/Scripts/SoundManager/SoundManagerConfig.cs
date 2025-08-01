using UnityEngine;

[CreateAssetMenu(fileName = "SoundManagerConfig", menuName = "Scriptable Objects/SoundManagerConfig")]
public class SoundManagerConfig : ScriptableObject
{
    [SerializeField]
    public string musicVcaName = "vca:/Music";
    [SerializeField]
    public string sfxVcaName = "vca:/Sfx";

    [SerializeField]
    public string sfxUiClickPath = "path";
    [SerializeField]
    public string sfxUiPointerPath = "path";

    [SerializeField]
    public string sfxSelectShipPath = "event:/Ship_select";

    public FMOD.Studio.EventInstance musicLoopInstance;
    public FMOD.Studio.VCA music_VCA;
    public FMOD.Studio.VCA sfx_VCA;
}
