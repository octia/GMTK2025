using System;
using PrimeTween;
using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [Inject]
    private SceneManager sceneManager;

    [Inject]
    private SoundManager soundManager;

    [SerializeField]
    private GameObject gameOverScreen;

    [SerializeField]
    private CanvasGroup terminationScreen;

    [SerializeField]
    private CanvasGroup scoresScreen;

    [SerializeField]
    private Button leaderboardSubmitButton;

    [SerializeField]
    private Button retryButton;

    private void Awake()
    {
        retryButton.onClick.AddListener(Retry);
        leaderboardSubmitButton.onClick.AddListener(SubmitToLeaderboard);
    }

    public void GameOver()
    {
        gameObject.SetActive(true);
        gameOverScreen.transform.localScale = Vector3.one * 0.02f;
        ScaleGameOverScreen(true);
        soundManager.PlayOneShot("event:/Danger!");
    }

    private void Retry()
    {
        ScaleGameOverScreen(false, sceneManager.LoadGameplay);
    }

    private void SubmitToLeaderboard()
    {
        terminationScreen.interactable = false;
        Sequence.Create()
            .Chain(Tween.Custom(1, 0, 0.4f, x => terminationScreen.alpha = x))
            .ChainCallback(() => terminationScreen.gameObject.SetActive(false))
            .ChainCallback(() => scoresScreen.gameObject.SetActive(true))
            .Chain(Tween.Custom(0, 1, 0.4f, x => scoresScreen.alpha = x));
    }

    private void ScaleGameOverScreen(bool enabled, Action onComplete = null)
    {
        var startScale = enabled ? 0.02f : 1;
        var endScale = enabled ? 1 : 0.02f;
        var yTween = Tween.ScaleY(gameOverScreen.transform, startScale, endScale, 0.2f);
        var xTween = Tween.ScaleX(gameOverScreen.transform, startScale, endScale, 0.4f);
        var firstTween = enabled ? yTween : xTween;
        var secondTween = enabled ? xTween : yTween;
        secondTween.OnComplete(onComplete);
        firstTween.Chain(secondTween);
    }
}
