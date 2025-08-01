using FMODUnity;
using PrimeTween;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [Inject]
    private SoundManager soundManager;
    [SerializeField] private GameObject goEnable;
    private bool bOptionsOpen = false;
    private bool bPaused = false;
    [SerializeField] private InputAction pauseAction;
    [SerializeField] private EventReference musicEventReference;

    private void Awake()
    {
        pauseAction.Enable();
    }
    private void Start()
    {
        soundManager.soundManagerConfig.music_VCA = FMODUnity.RuntimeManager.GetVCA(soundManager.soundManagerConfig.musicVcaName);
        soundManager.soundManagerConfig.sfx_VCA = FMODUnity.RuntimeManager.GetVCA(soundManager.soundManagerConfig.sfxVcaName);
        soundManager.PlayMusic(musicEventReference);
    }
    private void Update()
    {
        if (pauseAction.triggered)
        {
            PauseGame();
        }
    }
    public void ResumeGame()
    {
        Sequence.Create()
            .ChainDelay(0.8f)
            .OnComplete(() => PauseGame());
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
        //Book is popping out of nowhere
    }
}
