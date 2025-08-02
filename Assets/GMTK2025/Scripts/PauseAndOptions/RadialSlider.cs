using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class RadialSlider : Selectable, IDragHandler, IPointerDownHandler, ICanvasElement
{
    public enum FillOrigin { Top, Right, Bottom, Left }

    [SerializeField] private TMP_Text percentVolume;
    [SerializeField] private Image fillImage;
    [SerializeField] private RectTransform handle;
    [SerializeField] private bool clockwise = true;
    [SerializeField, Range(0, 1)] public float m_Value = 0f;
    public float fValue = 0f;

    public UnityEvent<float> onValueChanged = new UnityEvent<float>();

    public float value
    {
        get => m_Value;
        set => SetValue(value);
    }

    public void SetValue(float val)
    {
        val = Mathf.Clamp01(val);
        if (Mathf.Approximately(m_Value, val))
        {
            return;
        }

        m_Value = val;
        fValue = m_Value;
        UpdateVisuals();
        onValueChanged.Invoke(m_Value);
    }

    private void UpdateVisuals()
    {
        if (fillImage)
        {
            fillImage.fillAmount = m_Value;
        }

        if (handle)
        {
            float angle = (clockwise ? -360f : 360f) * m_Value;
            handle.localEulerAngles = new Vector3(0, 0, angle);
        }

        if(percentVolume)
        {
            percentVolume.SetText(Mathf.Round(m_Value * 100).ToString() + "%");
        }
    }

    public void OnPointerDown(PointerEventData eventData) => UpdateDrag(eventData);

    public void OnDrag(PointerEventData eventData) => UpdateDrag(eventData);

    private void UpdateDrag(PointerEventData eventData)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform, eventData.position, eventData.pressEventCamera, out localPos
        );

        float angle = Mathf.Atan2(localPos.y, -localPos.x) * Mathf.Rad2Deg;
        angle = (angle + 360f - 90f) % 360f; // Offset so 0 is at top

        float val = angle / 360f;
        if (!clockwise)
        {
            val = 1f - val;
        }

        SetValue(val);
    }

    public void Rebuild(CanvasUpdate executing) { }
    public void LayoutComplete() { }
    public void GraphicUpdateComplete() { }
}
