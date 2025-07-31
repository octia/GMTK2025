using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.Splines.Interpolators;

[RequireComponent(typeof(SplineContainer))]
public class Orbit : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 1.0f;

    private SplineContainer splineContainer;
    private Material material;
    private static int mainTextureId = Shader.PropertyToID("_BaseMap");

    private void Awake()
    {
        splineContainer = GetComponent<SplineContainer>();

        var renderer = GetComponent<MeshRenderer>();
        material = renderer.material;
    }

    void Update()
    {
        material.SetTextureOffset(mainTextureId, new Vector2(0, Time.time * scrollSpeed));
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
        SplineUtility.GetNearestPoint(splineContainer.Spline, p, out float3 nearest, out float t);
        nearestPoint = transform.TransformPoint(nearest);

        forward = transform.TransformDirection(splineContainer.Spline.EvaluateTangent(t)).normalized;

        var speedData = splineContainer.Spline.GetOrCreateFloatData("Speed");
        var knot = SplineUtility.ConvertIndexUnit(splineContainer.Spline, t, PathIndexUnit.Knot);
        // Ugly approximation using just x scaling, but probably fine
        speed = speedData.Evaluate(splineContainer.Spline, knot, new LerpFloat()) * transform.localScale.x;
    }
}
