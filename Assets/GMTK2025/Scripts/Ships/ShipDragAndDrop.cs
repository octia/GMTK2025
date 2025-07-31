using Reflex.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipDragAndDrop : MonoBehaviour
{
    [Inject] private readonly OrbitController orbitController;
    [SerializeField] private LayerMask shipLayerMask;
    [SerializeField] private Material ghostMaterial;

    Ship draggedShip = null;
    GameObject shipGhost = null;
    Orbit targetOrbit = null;

    public Orbit TargetOrbit
    {
        get => targetOrbit;
        set
        {
            if (targetOrbit == value)
            {
                return;
            }
            if (targetOrbit)
            {
                targetOrbit.State = Orbit.VisualState.Visible;
            }
            if (value)
            {
                value.State = Orbit.VisualState.Hightlighted;
            }
            targetOrbit = value;
        }
    }

    private void Update()
    {
        bool hoveringOverShip = MousePicking.PickObjectFromLayer(x => true, shipLayerMask, out Ship ship, out Vector3 _);

        if (hoveringOverShip && Mouse.current.leftButton.wasPressedThisFrame)
        {
            draggedShip = ship;
            shipGhost = Instantiate(draggedShip.gameObject);
            Destroy(shipGhost.GetComponent<Ship>());
            shipGhost.GetComponent<Rigidbody>().detectCollisions = false;
            foreach(var renderer in shipGhost.GetComponentsInChildren<MeshRenderer>())
            {
                renderer.sharedMaterial = ghostMaterial;
            }
            shipGhost.transform.rotation = Quaternion.identity;
        }
        if (draggedShip)
        {
            if (orbitController.PickUnlockedOrbitWithMouse(out Orbit orbit, out Vector3 hitPosition))
            {
                TargetOrbit = orbit;
                orbit.GetNearestPoint(hitPosition, out Vector3 nearestPoint, out Vector3 forward, out _);
                shipGhost.transform.SetPositionAndRotation(nearestPoint, Quaternion.LookRotation(draggedShip.ShipData.Clockwise ? -forward : forward, Vector3.up));
            }
            else
            {
                var viewPlane = new Plane(-Camera.main.transform.forward, shipGhost.transform.position);
                var mouseRay = Camera.main.ScreenPointToRay(Mouse.current.position.value);
                viewPlane.Raycast(mouseRay, out float distance);
                shipGhost.transform.position = mouseRay.GetPoint(distance);
                TargetOrbit = null;
            }
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (draggedShip && TargetOrbit)
            {
                draggedShip.ChangeOrbit(targetOrbit);
            }
            if (shipGhost)
            {
                Destroy(shipGhost);
            }
            draggedShip = null;
            TargetOrbit = null;
        }
    }
}
