using Reflex.Attributes;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private SceneType sceneTypeToLoad;

    [Inject]
    private SceneManager sceneManager;

    public void LoadScene()
    {
        sceneManager.LoadScene(sceneTypeToLoad);
    }
}
