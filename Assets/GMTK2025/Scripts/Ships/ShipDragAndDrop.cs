using Reflex.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipDragAndDrop : MonoBehaviour
{
    [Inject]
    SoundManager soundManager;

    [Inject]
    private readonly OrbitController orbitController;

    [SerializeField]
    private LayerMask shipLayerMask;

    [SerializeField]
    private Material ghostMaterial;

    [SerializeField]
    private float ghostSimulationTimestep = 0.1f;

    [SerializeField]
    private int maxSimulationSteps = 30;

    [SerializeField]
    private int stepsPerGhost = 2;

    [Inject]
    private UIManager uiManager;

    [Inject]
    private TimerManager timerManager;

    [Inject]
    private UpgradeManager upgradeManager;

    private Ship[] ghostShips = null;

    private Ship draggedShip = null;
    private Orbit targetOrbit = null;

    private bool allowedToInteract;
    public bool AllowedToInteract
    {
        get => allowedToInteract;
        private set
        {
            if (allowedToInteract && !value && draggedShip)
            {
                OnDeselect();
                UpdateVisualStates(null, null);
            }
            allowedToInteract = value;
        }
    }

    private void Awake()
    {
        ghostShips = new Ship[maxSimulationSteps / stepsPerGhost + 1];
    }

    private void Update()
    {
        AllowedToInteract = !timerManager.IsPaused && !upgradeManager.OnScreen;

        if (!AllowedToInteract)
        {
            return;
        }

        Camera cam = uiManager.UICamera == null ? Camera.main : uiManager.UICamera;
        bool hoveringOverShip = MousePicking.PickObjectFromLayer(
            x => x.Orbit,
            shipLayerMask,
            out Ship ship,
            out Vector3 _,
            cam
        );

        if (hoveringOverShip && Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnSelect(ship);
        }
        if (draggedShip)
        {
            OnDragging();
        }

        UpdateVisualStates(draggedShip, hoveringOverShip ? ship : null);
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            OnDeselect();
            UpdateVisualStates(null, null);
        }
    }

    private void OnSelect(Ship ship)
    {
        draggedShip = ship;
        //sfx
        soundManager.PlayOneShotWithStatus(draggedShip.shipData.Captain.sfxPath, "Idle");
        //
        ghostShips[0] = Instantiate(draggedShip.gameObject).GetComponent<Ship>();
        ghostShips[0].enabled = false;
        DestroyImmediate(ghostShips[0].GetComponent<Rigidbody>());
        foreach (var _renderer in ghostShips[0].GetComponentsInChildren<MeshRenderer>())
        {
            if ((shipLayerMask.value & (1 << _renderer.gameObject.layer)) == 0)
            {
                continue;
            }
            _renderer.sharedMaterial = ghostMaterial;
        }
        foreach (var particles in ghostShips[0].GetComponentsInChildren<ParticleSystem>())
        {
            DestroyImmediate(particles);
        }
        foreach (var trail in ghostShips[0].GetComponentsInChildren<TrailRenderer>())
        {
            DestroyImmediate(trail);
        }
        for (int i = 1; i < ghostShips.Length; i++)
        {
            ghostShips[i] = Instantiate(ghostShips[0].gameObject).GetComponent<Ship>();
        }
    }

    private void OnDragging()
    {
        if (
                orbitController.PickUnlockedOrbitWithMouse(
                    out Orbit orbit,
                    out Vector3 hitPosition
                )
            )
        {
            ghostShips[0].ChangeOrbit(orbit);
            ghostShips[0]
                .transform.SetPositionAndRotation(
                    draggedShip.transform.position,
                    draggedShip.transform.rotation
                );
            int ghostIndex = 1;
            for (int step = 0; step < maxSimulationSteps; step++)
            {
                ghostShips[0].SimulateMoveStep(0.1f, false, out Vector3 nearestPoint);
                bool duplicateGhostInStep = (step + 1) % stepsPerGhost == 0;

                if (duplicateGhostInStep)
                {
                    var ghostShip = ghostShips[ghostIndex++];
                    ghostShip.gameObject.SetActive(true);
                    ghostShip.transform.SetPositionAndRotation(
                        ghostShips[0].transform.position,
                        ghostShips[0].transform.rotation
                    );
                }
                if (
                    Vector3.SqrMagnitude(ghostShips[0].transform.position - nearestPoint)
                    <= 3f
                )
                {
                    break;
                }
            }
            for (int i = ghostIndex; i < ghostShips.Length; i++)
            {
                ghostShips[i].gameObject.SetActive(false);
            }
            targetOrbit = orbit;
        }
        else
        {
            // Draw ghost in view-parallel plane of dragged ship
            var viewPlane = new Plane(
                -Camera.main.transform.forward,
                draggedShip.transform.position
            );
            var mouseRay = Camera.main.ScreenPointToRay(Mouse.current.position.value);
            viewPlane.Raycast(mouseRay, out float distance);
            ghostShips[0]
                .transform.SetPositionAndRotation(
                    mouseRay.GetPoint(distance),
                    draggedShip.transform.rotation
                );
            for (int i = 1; i < ghostShips.Length; i++)
            {
                ghostShips[i].gameObject.SetActive(false);
            }
            targetOrbit = null;
        }
    }

    private void OnDeselect()
    {
        if (draggedShip && draggedShip.Orbit && targetOrbit)
        {
            draggedShip.ChangeOrbit(targetOrbit);
        }
        DestroyGhosts();
        draggedShip = null;
        targetOrbit = null;
    }

    private void DestroyGhosts()
    {
        foreach (var ghost in ghostShips)
        {
            if (ghost)
            {
                Destroy(ghost.gameObject);
            }
        }
    }

    public void UpdateVisualStates(Ship draggedShip, Ship hoveredShip)
    {
        foreach (var orbit in orbitController.GetUnlockedOrbits())
        {
            var bestShip = draggedShip ? draggedShip : hoveredShip;
            var clockwise = bestShip ? bestShip.ShipData.Clockwise : true;
            if (orbit == targetOrbit)
            {
                orbit.Clockwise = clockwise;
                orbit.State = Orbit.VisualState.Selected;
            }
            else if (
                (draggedShip && orbit == draggedShip.PreferredOrbit)
                || (hoveredShip && orbit == hoveredShip.PreferredOrbit)
            )
            {
                orbit.Clockwise = clockwise;
                if(orbit.State != Orbit.VisualState.Hightlighted)
                {
                    soundManager.PlayOneShot(soundManager.soundManagerConfig.sfxHoverPath);
                }
                orbit.State = Orbit.VisualState.Hightlighted;
            }
            else
            {
                orbit.State = Orbit.VisualState.Visible;
            }
        }
    }
}
