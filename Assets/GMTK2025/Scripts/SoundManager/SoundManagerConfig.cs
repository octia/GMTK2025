using UnityEngine;

[CreateAssetMenu(fileName = "SoundManagerConfig", menuName = "Scriptable Objects/SoundManagerConfig")]
public class SoundManagerConfig : ScriptableObject
{
    [SerializeField]
    private string musicVCAPath = "vca:/Music";

    [SerializeField]
    private string sfxVCAPath = "vca:/SFX";

    private FMOD.Studio.VCA _musicVCA;
    private FMOD.Studio.VCA _sfxVCA;

    public FMOD.Studio.VCA music_VCA
    {
        get
        {
            if (!_musicVCA.isValid())
                _musicVCA = FMODUnity.RuntimeManager.GetVCA(musicVCAPath);
            return _musicVCA;
        }
    }

    public FMOD.Studio.VCA sfx_VCA
    {
        get
        {
            if (!_sfxVCA.isValid())
                _sfxVCA = FMODUnity.RuntimeManager.GetVCA(sfxVCAPath);
            return _sfxVCA;
        }
    }
}
