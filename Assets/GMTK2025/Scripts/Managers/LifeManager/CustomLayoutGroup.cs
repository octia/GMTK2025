using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Custom layout group that arranges children in a spiral:
/// Up → Left → Down → Right → repeat.
/// Behaves like Unity's Horizontal/VerticalLayoutGroup for child sizing.
/// </summary>
[AddComponentMenu("Layout/Custom Layout Group")]
public class CustomLayoutGroup : LayoutGroup
{
    public float spacing = 5f;
    public bool childControlWidth = true;
    public bool childControlHeight = true;
    public bool childForceExpandWidth = false;
    public bool childForceExpandHeight = false;

    public float maxWidth = 0f;
    public float maxHeight = 0f;

    private float cellWidth;
    private float cellHeight;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        // Largest preferred width among children
        for (int i = 0; i < rectTransform.childCount; i++)
        {
            var child = rectTransform.GetChild(i) as RectTransform;
            if (child == null || !child.gameObject.activeSelf)
            {
                continue;
            }
            if (maxWidth ==0)
            {
                maxWidth = Mathf.Max(maxWidth, LayoutUtility.GetPreferredWidth(child));
            }
        }
        cellWidth = maxWidth;
    }

    public override void CalculateLayoutInputVertical()
    {
        for (int i = 0; i < rectTransform.childCount; i++)
        {
            var child = rectTransform.GetChild(i) as RectTransform;
            if (child == null || !child.gameObject.activeSelf)
            {
                continue;
            }
            if (maxWidth == 0)
            {
                maxHeight = Mathf.Max(maxHeight, LayoutUtility.GetPreferredHeight(child));
            }
        }
        cellHeight = maxHeight;
    }

    public override void SetLayoutHorizontal() => ArrangeChildren();
    public override void SetLayoutVertical() => ArrangeChildren();

    private enum Direction { Up, Left, Down, Right }

    private void ArrangeChildren()
    {
        if (rectTransform.childCount == 0)
        {
            return;
        }

        float parentHeight = rectTransform.rect.height - padding.vertical;
        float parentWidth = rectTransform.rect.width - padding.horizontal;

        // Base cell sizes (like standard layout)
        float w = cellWidth;
        float h = cellHeight;

        if (childForceExpandWidth && rectTransform.childCount > 0)
        {
            w = Mathf.Max(w, parentWidth / rectTransform.childCount);
        }
        if (childForceExpandHeight && rectTransform.childCount > 0)
        {
            h = Mathf.Max(h, parentHeight / rectTransform.childCount);
        }

        // Start at bottom-right
        float x = rectTransform.rect.width - padding.right - w;
        float y = rectTransform.rect.height - padding.top - h;
        //float y = padding.bottom;

        Direction dir = Direction.Down; // first move = up

        for (int i = 0; i < rectTransform.childCount; i++)
        {
            RectTransform child = rectTransform.GetChild(i) as RectTransform;
            if (child == null || !child.gameObject.activeSelf)
            {
                continue;
            }

            // Place child
            SetChildAlongAxis(child, 0, x, w);
            SetChildAlongAxis(child, 1, y, h);

            // Spiral logic
            switch (dir)
            {
                case Direction.Up:
                    y += h + spacing;
                    if (y + h > rectTransform.rect.height - padding.top)
                    {
                        dir = Direction.Right;
                        y = rectTransform.rect.height - padding.top - h;
                        x += w + spacing;
                    }
                    break;

                case Direction.Left:
                    x -= w + spacing;
                    if (x < padding.left)
                    {
                        dir = Direction.Up;
                        x = padding.left;
                        y += h + spacing;
                    }
                    break;

                case Direction.Down:
                    y -= h + spacing;
                    if (y < padding.bottom)
                    {
                        dir = Direction.Left;
                        y = padding.bottom;
                        x -= w + spacing;
                    }
                    break;

                case Direction.Right:
                    x += w + spacing;
                    if (x + w > rectTransform.rect.width - padding.right)
                    {
                        dir = Direction.Up;
                        x = rectTransform.rect.width - padding.right - w;
                        y -= h + spacing;
                    }
                    break;
            }
        }
    }

}
