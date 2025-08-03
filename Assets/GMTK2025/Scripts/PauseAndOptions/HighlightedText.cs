using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HighlightedText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI text;
    public Color normalColor;
    public Color highlightColor;
    public TMP_FontAsset normalFont;
    public TMP_FontAsset hoverFont;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        NotHovered();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = highlightColor;
        text.font = hoverFont;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        NotHovered();
    }

    private void NotHovered()
    {
        text.color = normalColor;
        text.font = normalFont;
    }
}
