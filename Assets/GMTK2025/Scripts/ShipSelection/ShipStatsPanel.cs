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
    private Sprite[] pipFillSpritesSpeed;

    [SerializeField]
    private Sprite[] pipFillSpritesSize;

    public void SetData(ShipData ship, float stayTime)
    {
        nameText.text = ship.name;
        sizePipImage.sprite = pipFillSpritesSize[(int)ship.ModelPrefab.GetComponent<ShipModel>().Size];
        speedPipImage.sprite = pipFillSpritesSpeed[(int)ship.Speed];
        directionImage.sprite = directionSprites[ship.Clockwise ? 0 : 1];
        if(ship.Clockwise)
        {
            directionImage.gameObject.GetComponent<RotateUI>().angularSpeed = new Vector3(0, 0, -10);
        }
        else
        {
            directionImage.gameObject.GetComponent<RotateUI>().angularSpeed = new Vector3(0, 0, 10);
        }

            profitText.text = $"${ship.Profit}";
        stayTimeText.text = ship.Temporary ? Mathf.RoundToInt(stayTime).ToString() : "∞";
    }
}
