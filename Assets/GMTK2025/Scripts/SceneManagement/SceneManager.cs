using System;
using Reflex.Attributes;
using UnityEngine;

public class SceneManager
{
    [Inject]
    private SceneManagerConfig config;

    public void LoadScene(SceneType sceneType)
    {
        switch (sceneType)
        {
            case SceneType.None:
                return;
            case SceneType.MainMenu:
                LoadMainMenu();
                break;
            case SceneType.Gameplay:
                LoadGameplay();
                break;
            case SceneType.Credits:
                LoadCredits();
                break;
            case SceneType.Exit:
                ExitGame();
                break;
        }
    }

    public void LoadCredits()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(config.CreditsSceneIndex);
    }

    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(config.MainMenuSceneIndex);
    }

    public void LoadGameplay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(config.GameplaySceneIndex);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
