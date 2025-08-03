using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HighlightedText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI text;
    [SerializeField]private Color normalColor;
    [SerializeField] private Color highlightColor;
    [SerializeField] private TMP_FontAsset normalFont;
    [SerializeField] private TMP_FontAsset hoverFont; [Header("Auto Size Settings")]
    [SerializeField] private float normalMaxFontSize = 54f;
    [SerializeField] private float hoverMaxFontSize = 74f;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        NotHovered();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = highlightColor;
        text.font = hoverFont;
        text.fontSizeMax = hoverMaxFontSize;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        NotHovered();
    }

    private void NotHovered()
    {
        text.color = normalColor;
        text.font = normalFont;
        text.fontSizeMax = normalMaxFontSize;
    }
}
