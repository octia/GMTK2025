using EditorAttributes;
using FMOD.Studio;
using FMODUnity;
using Reflex.Attributes;
using Unity.VisualScripting;
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

    [Inject]
    private ShipManagerConfig shipManagerConfig;

    private EventInstance orbitEventInstance;
    private bool bOnce = false;

    private Rigidbody rb;
    private float lastSpeed = 10;
    private Bounds modelBounds;

    public Orbit Orbit => orbit;
    public ShipData ShipData => shipData;
    public Orbit PreferredOrbit => preferredOrbit;
    [ShowInInspector] public float DistanceToOrbit { get; private set; }
    

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

            orbitEventInstance = soundManager.PlayLoop(
                soundManager.soundManagerConfig.sfxLoopShipFlyPath,
                shipData.ModelPrefab.GetComponent<ShipModel>().Size.ToString()
            );
            bOnce = true;
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
        float shipSpeed = shipManagerConfig.SpeedTypeToFloat[(int)shipData.Speed];

        if (orbit)
        {
            orbit.GetNearestPoint(
                transform.position,
                out nearestPoint,
                out Vector3 forward,
                out var orbitSpeed
            );
            if (shipData.Clockwise)
            {
                forward = -forward;
            }
            wantedForward = nearestPoint + forward * 4 - transform.position;
            speed = orbitSpeed * shipSpeed;
        }

        

        // Ships move towards their forward vector
        var newPosition = Vector3.MoveTowards(
            transform.position,
            transform.position + transform.forward * 100,
            timeStep * speed
        );

        // They turn towards a wanted forward vector, with a maximum angle change per fixed step
        var allowedForward = LimitRotation(
            transform.forward,
            wantedForward,
            timeStep * steerAngleChangeLimit * shipSpeed
        );

        // Purely visual up vector
        var calculatedUp = (allowedForward - transform.forward).normalized;
        var allowedUp = LimitRotation(
            transform.up,
            calculatedUp,
            timeStep * steerAngleChangeLimit * shipSpeed
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

        DistanceToOrbit = orbit ? Vector3.Distance(transform.position, nearestPoint) : Mathf.Infinity;

        UpdateOrbitSFX();

        lastSpeed = speed;
    }

    private void UpdateOrbitSFX()
    {
        if (DistanceToOrbit >= 1 && enabled && !bOnce)
        {
            //The loop of the engine
            orbitEventInstance = soundManager.PlayLoop(
                soundManager.soundManagerConfig.sfxLoopShipFlyPath,
                shipData.ModelPrefab.GetComponent<ShipModel>().Size.ToString()
            );
            bOnce = true;
        }
        else if (DistanceToOrbit >= 1 && enabled && bOnce)
        {
            if (orbitEventInstance.isValid())
            {
                orbitEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
            }
        }
        else if (DistanceToOrbit <= 1 && bOnce)
        {
            soundManager.StopLoop(orbitEventInstance);
            bOnce = false;
        }
    }

    public void DestroyShip()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (orbitEventInstance.isValid())
        {
            soundManager.StopLoop(orbitEventInstance);
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

        Bounds bounds = new Bounds(shipData.ModelPrefab.transform.position, Vector3.zero);
        foreach (var renderer in ShipData.ModelPrefab.GetComponentsInChildren<MeshRenderer>())
        {
            bounds.Encapsulate(renderer.bounds);
        }
        modelBounds = bounds;
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

    public bool IsCloseToCollision(out Vector3 scale)
    {
        var lengthScale = modelBounds.size.z + lastSpeed * 2.0f;
        scale = new Vector3(modelBounds.size.x * 1.4f, modelBounds.size.y * 1.4f, lengthScale);
        var hitColliders = Physics.OverlapBox(transform.position, scale * 0.5f, transform.rotation, 1 << gameObject.layer, QueryTriggerInteraction.Ignore);
        foreach (var collider in hitColliders)
        {
            var hitShip = collider.GetComponentInParent<Ship>();
            if (hitShip && hitShip != this)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (!ShipData || modelBounds.size == Vector3.zero)
        {
            return;
        }
        bool isClose = IsCloseToCollision(out Vector3 scale);
        Gizmos.color = isClose ? Color.red : Color.green;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, scale);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
