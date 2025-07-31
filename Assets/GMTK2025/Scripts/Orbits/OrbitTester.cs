using Reflex.Attributes;
using UnityEngine;

public class OrbitTester : MonoBehaviour
{
    [SerializeField] private float speedScale = 10;

    [Inject] private readonly OrbitController controller;

    private Orbit orbit;

    private void Update()
    {
        var newOrbit = controller.PickUnlockedOrbitWithMouse();
        if (orbit != null && orbit != newOrbit)
        {
            orbit.State = Orbit.VisualState.Visible;
        }
        if (newOrbit != null)
        {
            newOrbit.State = Orbit.VisualState.Hightlighted;
            orbit = newOrbit;
        }

        if (orbit)
        {
            orbit.GetNearestPoint(transform.position, out Vector3 nearestPoint, out Vector3 forward, out float speed);
            Debug.DrawLine(transform.position, nearestPoint, Color.red);
            Debug.DrawLine(nearestPoint, nearestPoint + forward * speed * speedScale, Color.purple);
        }
    }
}
