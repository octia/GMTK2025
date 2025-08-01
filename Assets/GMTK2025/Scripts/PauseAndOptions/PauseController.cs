using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject goEnable;
    private bool bOptionsOpen = false;
    private bool bPaused = false;
    [SerializeField] private InputAction pauseAction;

    private void Awake()
    {
        pauseAction.Enable();
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
            .ChainDelay(1)
            .OnComplete(() => PauseGame());
    }

    private void PauseGame()
    {
        bPaused = !bPaused;
        goEnable.SetActive(bPaused);
    }

    public void OptionsGame()
    {
        bOptionsOpen = !bOptionsOpen;
        //Book is popping out of nowhere
    }
}
