using EditorAttributes;
using PrimeTween;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using UnityEngine.Splines.Interpolators;

[RequireComponent(typeof(SplineContainer))]
public class Orbit : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 1.0f;
    [SerializeField] private Color visibleColor = Color.white;
    [FormerlySerializedAs("hightlightColor"), SerializeField] private Color highlightColor = Color.white;
    [SerializeField] private Color selectColor;

    float fillValue = 0.5f;

    /// <summary>
    /// Change this to change the visual state of the Orbit.
    /// </summary>
    public VisualState State
    {
        get
        {
            return state;
        }
        set
        {
            if (state != value)
            {
                OnVisualStateChange(value);
            }
            state = value;
        }
    }

    [SerializeField, DisableInPlayMode] private VisualState state = VisualState.Invisible;
    private SplineContainer splineContainer;
    private Material material;
    private Sequence visualStateChangeTween;
    private static readonly int mainTextureId = Shader.PropertyToID("_BaseMap");

#pragma warning disable IDE0051 // Remove unused private members
    [Button] private void SetVisualStateToInvisible() => State = VisualState.Invisible;
    [Button] private void SetVisualStateToVisible() => State = VisualState.Visible;
    [Button] private void SetVisualStateToHightlighted() => State = VisualState.Hightlighted;
#pragma warning restore IDE0051 // Remove unused private members


    private void Awake()
    {
        splineContainer = GetComponent<SplineContainer>();

        var renderer = GetComponent<MeshRenderer>();
        material = new Material(renderer.material);
        renderer.sharedMaterial = material;
    }

    private void Start()
    {
        OnVisualStateChange(State, true);
    }

    void Update()
    {
        material.SetTextureOffset(mainTextureId, new Vector2(fillValue, Time.time * scrollSpeed));
    }

    private void OnVisualStateChange(VisualState newState, bool instant = false)
    {
        var targetColor = newState switch
        {
            VisualState.Visible => visibleColor,
            VisualState.Hightlighted => highlightColor,
            VisualState.Selected => selectColor,
            _ => new Color(1, 1, 1, 0),
        };
        var targetFill = newState switch
        {
            VisualState.Hightlighted => 0.6f,
            VisualState.Selected => 0.999f,
            _ => 0.3f,
        };

        visualStateChangeTween.Complete();
        visualStateChangeTween = Sequence.Create()
            .Chain(Tween.MaterialColor(material, targetColor, 0.2f))
            .Group(Tween.Custom(fillValue, targetFill, 0.2f, x => fillValue = x));

        if (instant)
        {
            visualStateChangeTween.Complete();
        }
    }

    /// <summary>
    /// Finds nearest point on orbit.
    /// </summary>
    /// <param name="point">Point in world coordinates</param>
    /// <param name="nearestPoint">Point on orbit</param>
    /// <param name="forward">Forward vector on orbit</param>
    /// <param name="speed">Speed at point on orbit</param>
    public void GetNearestPoint(Vector3 point, out Vector3 nearestPoint, out Vector3 forward, out float speed)
    {
        var p = transform.InverseTransformPoint(point);
        SplineUtility.GetNearestPoint(splineContainer.Spline, p, out float3 nearest, out float t, 1, 2);
        nearestPoint = transform.TransformPoint(nearest);

        forward = transform.TransformDirection(splineContainer.Spline.EvaluateTangent(t)).normalized;

        var speedData = splineContainer.Spline.GetOrCreateFloatData("Speed");
        var knot = SplineUtility.ConvertIndexUnit(splineContainer.Spline, t, PathIndexUnit.Knot);
        // Ugly approximation using just x scaling, but probably fine
        speed = speedData.Evaluate(splineContainer.Spline, knot, new LerpFloat()) * transform.localScale.x;
    }

    public enum VisualState
    {
        Invisible,
        Visible,
        Hightlighted,
        Selected
    }
}
