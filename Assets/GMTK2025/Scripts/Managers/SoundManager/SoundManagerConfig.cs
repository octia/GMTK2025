using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

[CreateAssetMenu(
    fileName = "SoundManagerConfig",
    menuName = "Scriptable Objects/SoundManagerConfig"
)]
public class SoundManagerConfig : ScriptableObject
{
    [SerializeField]
    public string musicVcaName = "vca:/Music";

    [SerializeField]
    public string sfxVcaName = "vca:/Sfx";
    public float fVolumeMusic = 0f;
    public float fVolumeSfx = 0f;

    [SerializeField]
    public string sfxUiClickPath = "path";

    [SerializeField]
    public string sfxUiPointerPath = "path";

    [SerializeField]
    public string sfxTextPath = "event:/Text";

    [SerializeField]
    public string sfxSelectShipPath = "event:/Ship_select";

    [SerializeField]
    public string sfxLoopShipFlyPath = "event:/Ship_fly";

    [SerializeField]
    public string sfxHoverPath = "event:/Orbit_hover";

    [SerializeField]
    public string sfxExplosionPath = "event:/Ship_crash";

    [SerializeField]
    public string sfxErrorPath = "event:/Error";

    [SerializeField]
    public string sfxUpgradePath = "event:/Upgrade";

    [SerializeField]
    public string sfxSlowMotionPath = "event:/Slowmotion";

    [SerializeField]
    public string sfxNewLifePath = "event:/Getting life";

    [SerializeField]
    public string sfxDirectionChangePath = "event:/Direction change";

    public FMOD.Studio.EventInstance musicLoopInstance;
    public List<EventInstance> activeLoopedSounds = new List<EventInstance>();
    public FMOD.Studio.VCA music_VCA;
    public FMOD.Studio.VCA sfx_VCA;
}
