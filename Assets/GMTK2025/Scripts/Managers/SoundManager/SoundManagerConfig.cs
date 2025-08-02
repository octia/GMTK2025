using System.Collections.Generic;
using FMOD.Studio;
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
    [SerializeField]
    public string sfxLoopShipFlyPath = "event:/Ship_fly";
    [SerializeField]
    public string sfxHoverPath = "event:/Orbit_hover";
    public bool bSfxHoverOnce = false;

    public FMOD.Studio.EventInstance musicLoopInstance;
    public List<EventInstance> activeLoopedSounds = new List<EventInstance>();
    public FMOD.Studio.VCA music_VCA;
    public FMOD.Studio.VCA sfx_VCA;
}
