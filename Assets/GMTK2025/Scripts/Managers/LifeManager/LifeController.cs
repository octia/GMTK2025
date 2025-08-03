using System.Collections.Generic;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

public class LifeController : MonoBehaviour
{
    [SerializeField]
    private Transform heartImagesContainer;

    [SerializeField]
    private GameObject heartImagePrefab;

    [Inject]
    private LifeManager lifeManager;

    // ...
    private List<Image> heartImages = new List<Image>();

    private void Awake()
    {
        lifeManager.OnLifeCountChanged += OnLivesChanged;
        lifeManager.LifeController = this;
        lifeManager.GainMaxLife(lifeManager.Config.MaxLifeCount);
    }

    private void OnDisable()
    {
        lifeManager.OnLifeCountChanged -= OnLivesChanged;
    }

    private void OnLivesChanged()
    {
        Debug.Log("LIFE CHANGED");
        VisualLifeChange();
    }

    public void VisualLifeChange()
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            heartImages[i].sprite = i < lifeManager.LifeCount ? lifeManager.Config.heartSprite[0] : lifeManager.Config.heartSprite[1];
        }
    }

    public void AddHeartImages(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newHeartImage = Instantiate(heartImagePrefab, heartImagesContainer);
            heartImages.Add(newHeartImage.GetComponent<Image>());
        }
    }
}
