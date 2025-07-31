using EditorAttributes;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
public class OrbitGenerator : MonoBehaviour
{
    [SerializeField] private float scale = 1.0f;
    [SerializeField, Clamp(0, 1)] private float eccentricity = 1.0f;
    [SerializeField] private int pointCount = 40;
    [SerializeField] private float meshRadius = 0.1f;

    [Button]
    public void ApplyToSpline()
    {
        var splineContainer = GetComponent<SplineContainer>();

        splineContainer.Spline.Clear();
        var speedData = new SplineData<float>();
        for(int i = 0; i < pointCount; i++)
        {
            float t = i / (float)pointCount;
            var position = GetOrbitalPosition(scale, eccentricity, t);
            var floatPosition = new float3(position.x, 0, position.y);
            splineContainer.Spline.Add(new BezierKnot(floatPosition), TangentMode.AutoSmooth);
            speedData.Add(i, position.z);
        }
        splineContainer.Spline.SetFloatData("Speed", speedData);
    }

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
