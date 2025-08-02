using System.Collections.Generic;
using System.Linq;
using Reflex.Attributes;
using UnityEngine;

public class OrbitController : MonoBehaviour
{
    [SerializeField]
    private OrbitWithData[] orbitsWithData;

    [SerializeField]
    private float mousePickRadius = 1.0f;

    [SerializeField]
    private LayerMask orbitLayerMask;

    [Inject]
    private UIManager uiManager;

    private RaycastHit[] hits = null;

    public IEnumerable<Orbit> GetUnlockedOrbits() =>
        orbitsWithData.Where(x => x.Unlocked).Select(x => x.Orbit);

    public IEnumerable<Orbit> GetLockedOrbits() =>
        orbitsWithData.Where(x => x.Unlocked).Select(x => x.Orbit);

    public int GetOrbitPrice(Orbit orbit) => GetMatchingOrbitData(orbit).Price;

    public bool IsOrbitUnlocked(Orbit orbit) => GetMatchingOrbitData(orbit).Unlocked;

    public void UnlockOrbit(Orbit orbit)
    {
        var orbitWithData = GetMatchingOrbitData(orbit);
        orbitWithData.Unlocked = true;
        orbitWithData.Orbit.State = Orbit.VisualState.Visible;
    }

    /// <summary>
    /// Tries to find an orbit close to the mouse coursor
    /// </summary>
    /// <param name="orbit"></param>
    /// <param name="hitPosition"></param>
    /// <returns>Whether an orbit was found or not</returns>
    public bool PickUnlockedOrbitWithMouse(out Orbit orbit, out Vector3 hitPosition)
    {
        Camera cam = uiManager.UICamera == null ? Camera.main : uiManager.UICamera;
        return MousePicking.PickObjectFromLayer(
            IsOrbitUnlocked,
            orbitLayerMask,
            out orbit,
            out hitPosition,
            cam
        );
    }

    private OrbitWithData GetMatchingOrbitData(Orbit orbit) =>
        orbitsWithData.First(x => x.Orbit == orbit);

    private void Start()
    {
        foreach (var orbit in GetUnlockedOrbits())
        {
            orbit.State = Orbit.VisualState.Visible;
        }
    }

    [System.Serializable]
    private class OrbitWithData
    {
        public Orbit Orbit;
        public bool Unlocked;
        public int Price;
    }
}
