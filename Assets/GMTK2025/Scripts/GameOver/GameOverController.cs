using UnityEngine;

public class GameOverController : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverScreen;

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }
}
