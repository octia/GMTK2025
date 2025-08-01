using Reflex.Attributes;
using TMPro;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;

    [Inject]
    private LifeManager lifeManager;

    private void Awake()
    {
        lifeManager.OnLifeCountChanged += OnLivesChanged;
    }

    private void OnDisable()
    {
        lifeManager.OnLifeCountChanged -= OnLivesChanged;
    }

    private void OnLivesChanged()
    {
        text.text = $"Lives: {lifeManager.LifeCount}";
        Debug.Log("Current life count: " + lifeManager.LifeCount);
    }
}
