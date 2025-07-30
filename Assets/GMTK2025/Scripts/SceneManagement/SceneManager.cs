using Reflex.Attributes;

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
        }
    }

    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(config.MainMenuSceneIndex);
    }

    public void LoadGameplay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(config.GameplaySceneIndex);
    }
}
