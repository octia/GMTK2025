using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private Orbit orbit;
    [SerializeField] private ShipData shipData;

    [SerializeField] private float steerAngleChangeLimit = 180;
    [SerializeField] private float upVectorBlendFactor = 0.1f;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        orbit.GetNearestPoint(transform.position, out Vector3 nearestPoint, out Vector3 forward, out float speed);
        if (!shipData.Clockwise)
        {
            forward = -forward;
        }

        // Ships move towards their forward vector
        var newPosition = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Time.deltaTime * speed * shipData.Speed);

        // They turn towards a wanted forward vector, with a maximum angle change per fixed step
        var wantedForward = nearestPoint + forward * 4 - transform.position;
        var allowedForward = LimitRotation(transform.forward, wantedForward, Time.fixedDeltaTime * steerAngleChangeLimit * shipData.Speed);

        // Purely visual up vector
        var calculatedUp = (allowedForward - transform.forward).normalized;
        var allowedUp = LimitRotation(transform.up, calculatedUp, Time.fixedDeltaTime * steerAngleChangeLimit * shipData.Speed);

        var newRotation = Quaternion.LookRotation(allowedForward, allowedUp + Vector3.up * upVectorBlendFactor);
        rb.Move(newPosition, newRotation);
    }

    private Vector3 LimitRotation(Vector3 from, Vector3 to, float maxAngle)
    {
        var rotation = Quaternion.FromToRotation(from, to);
        rotation.ToAngleAxis(out float angle, out Vector3 axis);
        var clampedAngle = Mathf.Clamp(angle, -maxAngle, maxAngle);
        var clampedRotation = Quaternion.AngleAxis(clampedAngle, axis);
        var clampedTo = clampedRotation * from;
        return clampedTo;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Ships collided! {gameObject.name} <-> {collision.gameObject.name}");
    }
}
