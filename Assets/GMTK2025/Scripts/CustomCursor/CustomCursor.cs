using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    [SerializeField]
    private Vector2 offset;

    [SerializeField]
    private Sprite[] cursorSprites;

    [SerializeField]
    private Image cursorImage;

    [HideInInspector]
    public bool TargetMode;

    // Input actions
    private InputAction cursorPos;

    // Initial
    private RectTransform rectParent;

    private void Awake()
    {
        Cursor.visible = false;

        // Get member variables
        cursorPos = InputSystem.actions.FindAction("Point");

        rectParent = transform.parent.GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, cursorPos.ReadValue<Vector2>(), null, out anchoredPos);
        GetComponent<RectTransform>().anchoredPosition = anchoredPos + offset;
    }

    public void ChangeCursorSprite(int i)
    {
        if (TargetMode)
        {
            cursorImage.sprite = cursorSprites[3];
            return;
        }

        cursorImage.sprite = cursorSprites[i];
    }
}