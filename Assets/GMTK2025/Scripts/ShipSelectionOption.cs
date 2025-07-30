using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShipSelectionOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform statsPoint;

    [Space]
    [SerializeField] private GameObject highlightBackground;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private Animator orbitAnim;

    [Header("Visual")]
    [SerializeField] private Vector2 onHoverScale;
    [Tooltip("Time for the hover to occur")]
    [SerializeField] private float hoverAnimTime;
    [SerializeField] private float orbitAnimHoverSpeed;

    bool mouseOver;

    // Initial
    private Vector2 defaultScale;

    private void Start()
    {
        // Grabbing initial variables
        defaultScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;

        // Visual        
        Tween.Scale(transform, endValue: onHoverScale, hoverAnimTime);

        orbitAnim.speed = orbitAnimHoverSpeed;

        highlightBackground.SetActive(true);
        statsPanel.SetActive(true);
        statsPanel.transform.position = statsPoint.position;
        statsPanel.GetComponent<TransformFollower>().FollowTarget = statsPoint;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;

        // Visual
        Tween.Scale(transform, endValue: defaultScale, hoverAnimTime);

        orbitAnim.speed = 1;

        highlightBackground.SetActive(false);
        statsPanel.SetActive(false);
    }
}
