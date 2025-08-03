using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipStatsPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;

    [SerializeField]
    private Image sizePipImage;

    [SerializeField]
    private Image speedPipImage;

    [SerializeField]
    private Image directionImage;

    [SerializeField]
    private TMP_Text profitText;

    [SerializeField]
    private TMP_Text stayTimeText;

    [SerializeField]
    private Sprite[] directionSprites;

    [SerializeField]
    private Sprite[] pipFillSprites;

    public void SetData(ShipData ship, float stayTime)
    {
        nameText.text = ship.name;
        sizePipImage.sprite = pipFillSprites[(int)ship.ModelPrefab.GetComponent<ShipModel>().Size];
        speedPipImage.sprite = pipFillSprites[(int)ship.Speed];
        directionImage.sprite = directionSprites[ship.Clockwise ? 0 : 1];
        profitText.text = $"${ship.Profit}";
        stayTimeText.text = ship.Temporary ? Mathf.RoundToInt(stayTime).ToString() : "∞";
    }
}
