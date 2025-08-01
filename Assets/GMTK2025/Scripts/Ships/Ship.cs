using Reflex.Attributes;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField]
    private Orbit orbit;

    [SerializeField]
    private ShipData shipData;

    [SerializeField]
    private float steerAngleChangeLimit = 180;

    [SerializeField]
    private float upVectorBlendFactor = 0.1f;

    [Inject]
    private TimerManager timerManager;

    [Inject]
    private ShipCollisionManager shipCollisionManager;

    [Inject]
    private FinanceManager financeManager;

    private Rigidbody rb;

    public ShipData ShipData => shipData;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (timerManager == null)
        {
            Injector.Inject(this);
        }
    }

    public void ChangeOrbit(Orbit orbit)
    {
        this.orbit = orbit;
    }

    private void FixedUpdate()
    {
        SimulateMoveStep(Time.fixedDeltaTime * timerManager.TimeScale, true, out _);
        financeManager.GenerateShipProfit(Time.fixedDeltaTime * timerManager.TimeScale, shipData.Profit, shipData.Speed);
    }

    public void SimulateMoveStep(float timeStep, bool useRigidbody, out Vector3 nearestPoint)
    {
        orbit.GetNearestPoint(
            transform.position,
            out nearestPoint,
            out Vector3 forward,
            out float speed
        );
        if (shipData.Clockwise)
        {
            forward = -forward;
        }

        // Ships move towards their forward vector
        var newPosition = Vector3.MoveTowards(
            transform.position,
            transform.position + transform.forward * 100,
            timeStep * speed * shipData.Speed
        );

        // They turn towards a wanted forward vector, with a maximum angle change per fixed step
        var wantedForward = nearestPoint + forward * 4 - transform.position;
        var allowedForward = LimitRotation(
            transform.forward,
            wantedForward,
            timeStep * steerAngleChangeLimit * shipData.Speed
        );

        // Purely visual up vector
        var calculatedUp = (allowedForward - transform.forward).normalized;
        var allowedUp = LimitRotation(
            transform.up,
            calculatedUp,
            timeStep * steerAngleChangeLimit * shipData.Speed
        );

        var newRotation = Quaternion.LookRotation(
            allowedForward,
            allowedUp + Vector3.up * upVectorBlendFactor
        );

        if (useRigidbody)
        {
            rb.Move(newPosition, newRotation);
        }
        else
        {
            transform.SetLocalPositionAndRotation(newPosition, newRotation);
        }
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
        var point = collision.contacts[0].point;
        var ship1 = this;
        var ship2 = collision.gameObject.GetComponent<Ship>();
        shipCollisionManager.CollisionHappened(ship1, ship2, point);
    }

    internal void InitializeWith(ShipData ship)
    {
        shipData = ship;
        // todo: CHANGE THIS!!!!
        orbit = FindAnyObjectByType<Orbit>();
    }
}
