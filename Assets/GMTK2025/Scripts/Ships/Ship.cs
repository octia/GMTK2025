using FMOD.Studio;
using FMODUnity;
using Reflex.Attributes;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField]
    private Orbit orbit;

    [SerializeField]
    private Orbit preferredOrbit;

    [SerializeField]
    private ShipData shipData;

    [SerializeField]
    private GameObject modelInstance;

    [SerializeField]
    private float steerAngleChangeLimit = 180;

    [SerializeField]
    private float upVectorBlendFactor = 0.1f;

    [Inject]
    private TimerManager timerManager;

    [Inject]
    private ShipCollisionManager shipCollisionManager;

    [Inject]
    private SoundManager soundManager;
    private EventInstance orbitEventInstance;
    private bool bOnce = false;

    private Rigidbody rb;
    private float lastSpeed = 10;

    public Orbit Orbit => orbit;
    public ShipData ShipData => shipData;
    public Orbit PreferredOrbit => preferredOrbit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (timerManager == null)
        {
            Injector.Inject(this);
        }
    }

    private void Start()
    {
        if (shipData && enabled)
        {
            ReinstantiateShipModel();
        }
    }

    public void ChangeOrbit(Orbit orbit)
    {
        this.orbit = orbit;
    }

    public void ChangePreferredOrbit(Orbit orbit)
    {
        preferredOrbit = orbit;
    }

    private void FixedUpdate()
    {
        SimulateMoveStep(Time.fixedDeltaTime * timerManager.TimeScale, true, out _);
    }

    public void SimulateMoveStep(float timeStep, bool useRigidbody, out Vector3 nearestPoint)
    {
        float speed = lastSpeed;
        var wantedForward = transform.forward;
        nearestPoint = transform.position;

        if (orbit)
        {
            orbit.GetNearestPoint(
                transform.position,
                out nearestPoint,
                out Vector3 forward,
                out speed
            );
            if (shipData.Clockwise)
            {
                forward = -forward;
            }
            wantedForward = nearestPoint + forward * 4 - transform.position;
        }

        // Ships move towards their forward vector
        var newPosition = Vector3.MoveTowards(
            transform.position,
            transform.position + transform.forward * 100,
            timeStep * speed * shipData.Speed
        );

        // They turn towards a wanted forward vector, with a maximum angle change per fixed step
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
        SfxOrbit(nearestPoint, useRigidbody);
        lastSpeed = speed;
    }

    private void SfxOrbit(Vector3 nearestPoint, bool useRigidbody)
    {
        Vector3 distanceToTarget = this.transform.position - nearestPoint;
        if (Vector3.SqrMagnitude(distanceToTarget) >= 1 && useRigidbody && !bOnce)
        {
            orbitEventInstance = soundManager.PlayLoop(soundManager.soundManagerConfig.sfxLoopShipFlyPath, shipData.Size);
            bOnce = true;
        }
        else if(Vector3.SqrMagnitude(distanceToTarget) >= 1 && useRigidbody && bOnce)
        {
            if (orbitEventInstance.isValid())
            {
                orbitEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
            }
        }
        else if (Vector3.SqrMagnitude(distanceToTarget) <= 1 && bOnce)
        {
            soundManager.StopLoop(orbitEventInstance);
            bOnce = false;
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

    private void ReinstantiateShipModel()
    {
        Destroy(modelInstance);
        modelInstance = Instantiate(ShipData.ModelPrefab, transform);
        modelInstance.name = "ModelInstance";
        modelInstance.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        SetGameobjectLayerRecursive(modelInstance, gameObject.layer);
    }

    private void SetGameobjectLayerRecursive(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform child in gameObject.transform)
        {
            SetGameobjectLayerRecursive(child.gameObject, layer);
        }
    }

    internal void InitializeWith(ShipData ship, Orbit preferredOrbit)
    {
        shipData = ship;
        this.preferredOrbit = preferredOrbit;
        orbit = preferredOrbit;
        ReinstantiateShipModel();
    }
}
