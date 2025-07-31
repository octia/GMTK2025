using UnityEngine;

public class OrbitTester : MonoBehaviour
{
    [SerializeField] private Orbit orbit;
    [SerializeField] private float speedScale = 10;

    private void Update()
    {
        orbit.GetNearestPoint(transform.position, out Vector3 nearestPoint, out Vector3 forward, out float speed);
        Debug.DrawLine(transform.position, nearestPoint, Color.red);
        Debug.DrawLine(nearestPoint, nearestPoint + forward * speed * speedScale, Color.purple);
    }
}
