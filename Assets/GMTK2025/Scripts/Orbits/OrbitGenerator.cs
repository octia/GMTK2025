using EditorAttributes;
using Unity.Mathematics;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
public class OrbitGenerator : MonoBehaviour
{
    [SerializeField] private float scale = 1.0f;
    [SerializeField, Clamp(0, 1)] private float eccentricity = 1.0f;
    [SerializeField] private int pointCount = 40;

    public float Scale => scale;
    public float Eccentricity => eccentricity;

#if UNITY_EDITOR
    [Button]
    public void ApplyToSpline()
    {
        var splineContainer = GetComponent<SplineContainer>();

        if (splineContainer.Spline != null)
        {
            splineContainer.RemoveSpline(splineContainer.Spline);
        }
        splineContainer.AddSpline();
        splineContainer.Spline.Closed = true;

        var speedData = new SplineData<float>();
        speedData.PathIndexUnit = PathIndexUnit.Knot;
        for (int i = 0; i < pointCount; i++)
        {
            float t = i / (float)pointCount;
            var position = GetOrbitalPosition(scale, eccentricity, t);
            var floatPosition = new float3(position.x, 0, position.y);
            splineContainer.Spline.Add(new BezierKnot(floatPosition), TangentMode.AutoSmooth);
            speedData.Add(i, position.z);
        }
        splineContainer.Spline.SetFloatData("Speed", speedData);

        PrefabUtility.RecordPrefabInstancePropertyModifications(splineContainer);
    }
#endif

    private static Vector3 GetOrbitalPosition(float scale, float eccentricity, float t)
    {
        var angle = 2 * Mathf.PI * t;

        // Ellipse offset to focal
        var a = scale;
        var x = a * Mathf.Cos(angle) - scale * eccentricity;

        var b = a * Mathf.Sqrt(1 - eccentricity * eccentricity);
        var y = b * Mathf.Sin(angle);

        // Vis-viva equation
        var r = Mathf.Sqrt(x * x + y * y);
        var speed = Mathf.Sqrt(2.0f / r - 1.0f / a);
        return new Vector3(x, y, speed);
    }
}
