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

    private Ship[] ghostShips = null;

    private Ship draggedShip = null;
    private Orbit targetOrbit = null;
    private bool shouldUpdateVisualStates = false;

    private void Awake()
    {
        ghostShips = new Ship[maxSimulationSteps / stepsPerGhost + 1];
    }

    private void Update()
    {
        Camera cam = uiManager.UICamera == null ? Camera.main : uiManager.UICamera;
        bool hoveringOverShip = MousePicking.PickObjectFromLayer(
            x => true,
            shipLayerMask,
            out Ship ship,
            out Vector3 _,
            cam
        );

        if (!timerManager.IsPaused)
        {
            if (hoveringOverShip && Mouse.current.leftButton.wasPressedThisFrame)
            {
                draggedShip = ship;
                //sfx
                soundManager.PlayOneShotWithStatus(draggedShip.shipData.Captain.sfxPath, "Idle");
                //
                ghostShips[0] = Instantiate(draggedShip.gameObject).GetComponent<Ship>();
                ghostShips[0].enabled = false;
                DestroyImmediate(ghostShips[0].GetComponent<Rigidbody>());
                foreach (var renderer in ghostShips[0].GetComponentsInChildren<MeshRenderer>())
                {
                    draggedShip = ship;

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

                    for (int i = 1; i < ghostShips.Length; i++)
                    {
                        ghostShips[i] = Instantiate(ghostShips[0].gameObject).GetComponent<Ship>();
                    }
                }
            }
            if (draggedShip)
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
            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                if (draggedShip && targetOrbit)
                {
                    draggedShip.ChangeOrbit(targetOrbit);
                }
                foreach (var ghost in ghostShips)
                {
                    if (ghost)
                    {
                        Destroy(ghost.gameObject);
                    }
                }
                draggedShip = null;
                targetOrbit = null;
            }
        }

        if (shouldUpdateVisualStates)
        {
            foreach (var orbit in orbitController.GetUnlockedOrbits())
            {
                var bestShip = draggedShip ? draggedShip : ship;
                if (orbit == targetOrbit)
                {
                    orbit.Clockwise = bestShip.ShipData.Clockwise;
                    orbit.State = Orbit.VisualState.Selected;
                }
                else if (
                    (draggedShip && orbit == draggedShip.PreferredOrbit)
                    || (hoveringOverShip && orbit == ship.PreferredOrbit)
                )
                {
                    orbit.Clockwise = bestShip.ShipData.Clockwise;
                    orbit.State = Orbit.VisualState.Hightlighted;

                    //SFX
                    if (
                        hoveringOverShip
                        && orbit == ship.PreferredOrbit
                        && !soundManager.soundManagerConfig.bSfxHoverOnce
                    )
                    {
                        soundManager.PlayOneShot(soundManager.soundManagerConfig.sfxHoverPath);
                        soundManager.soundManagerConfig.bSfxHoverOnce = true;
                    }
                }
                else
                {
                    orbit.State = Orbit.VisualState.Visible;
                }
            }
        }

        // Important to delay update by one frame
        // That way, orbits will be deselected after all input stops
        shouldUpdateVisualStates = draggedShip || hoveringOverShip;

        //SFX-needed
        if (!hoveringOverShip)
        {
            soundManager.soundManagerConfig.bSfxHoverOnce = false;
        }
    }
}
