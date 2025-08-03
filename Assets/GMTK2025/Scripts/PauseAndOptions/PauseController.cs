using FMODUnity;
using PrimeTween;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public bool bIsOnMainMenu = false;

    [Inject]
    private SoundManager soundManager;

    [SerializeField]
    private GameObject goEnable;

    [SerializeField]
    private GameObject goOptions;

    [SerializeField]
    private GameObject goPause;
    private bool bOptionsOpen = false;
    private bool bPaused = false;

    [SerializeField]
    private InputAction pauseAction;

    [SerializeField]
    private EventReference musicEventReference;

    private void Awake()
    {
        pauseAction.Enable();
    }

    private void Start()
    {
        if (!bIsOnMainMenu)
        {
            if (!soundManager.soundManagerConfig.musicLoopInstance.isValid())
            {
                soundManager.soundManagerConfig.music_VCA = FMODUnity.RuntimeManager.GetVCA(
                    soundManager.soundManagerConfig.musicVcaName
                );
                soundManager.soundManagerConfig.sfx_VCA = FMODUnity.RuntimeManager.GetVCA(
                    soundManager.soundManagerConfig.sfxVcaName
                );
                soundManager.PlayMusic(musicEventReference);
            }
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            soundManager.soundManagerConfig.music_VCA = FMODUnity.RuntimeManager.GetVCA(
                soundManager.soundManagerConfig.musicVcaName
            );
            soundManager.soundManagerConfig.sfx_VCA = FMODUnity.RuntimeManager.GetVCA(
                soundManager.soundManagerConfig.sfxVcaName
            );
            soundManager.PlayMusic(musicEventReference);
        }
    }

    private void Update()
    {
        if (pauseAction.triggered && !bIsOnMainMenu)
        {
            PauseGame();
        }
    }

    public void ResumeGame()
    {
        Sequence.Create().ChainDelay(0.8f).OnComplete(() => PauseGame());
    }

    private void PauseGame()
    {
        bPaused = !bPaused;
        goEnable.SetActive(bPaused);
        soundManager.PausedGameVolume(bPaused);
    }

    public void OptionsGame()
    {
        bOptionsOpen = !bOptionsOpen;
        goOptions.SetActive(bOptionsOpen);
        goPause.SetActive(!bOptionsOpen);
        //Book is popping out of nowhere
    }
}
