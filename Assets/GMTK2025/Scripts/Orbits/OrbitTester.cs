using Reflex.Attributes;
using UnityEngine;

public class OrbitTester : MonoBehaviour
{
    [SerializeField] private float speedScale = 10;

    [SerializeField] private Orbit orbit;

    private void Update()
    {
        if (orbit)
        {
            orbit.GetNearestPoint(transform.position, out Vector3 nearestPoint, out Vector3 forward, out float speed);
            Debug.DrawLine(transform.position, nearestPoint, Color.red);
            Debug.DrawLine(nearestPoint, nearestPoint + forward * speed * speedScale, Color.purple);
        }
    }
}
