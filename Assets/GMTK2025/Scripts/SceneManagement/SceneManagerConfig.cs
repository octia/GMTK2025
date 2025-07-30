using UnityEngine;

[CreateAssetMenu(menuName = "GMTK2025/Config/SceneManagerConfig", fileName = "SceneManagerConfig")]
public class SceneManagerConfig : ScriptableObject
{
    [field: SerializeField]
    public int MainMenuSceneIndex { get; private set; } = 0;

    [field: SerializeField]
    public int GameplaySceneIndex { get; private set; } = 1;
}
