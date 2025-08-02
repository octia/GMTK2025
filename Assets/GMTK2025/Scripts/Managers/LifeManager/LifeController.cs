using Reflex.Attributes;
using TMPro;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;
    [SerializeField]
    private UnityEngine.UI.Image[] heartsImages = new UnityEngine.UI.Image[3];

    [Inject]
    private LifeManager lifeManager;

    private void Awake()
    {
        lifeManager.OnLifeCountChanged += OnLivesChanged;
        VisualLifeChange();
    }

    private void OnDisable()
    {
        lifeManager.OnLifeCountChanged -= OnLivesChanged;
    }

    private void OnLivesChanged()
    {
        text.text = $"Lives: {lifeManager.LifeCount}";
        VisualLifeChange();
    }

    public void VisualLifeChange()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i < lifeManager.LifeCount)
            {
                heartsImages[i].sprite = lifeManager.config.heartSprite[0];
            }
            else
            {
                heartsImages[i].sprite = lifeManager.config.heartSprite[1];
            }
        }
    }
}
