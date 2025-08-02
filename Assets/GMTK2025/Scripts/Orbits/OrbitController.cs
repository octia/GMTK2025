using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitController : MonoBehaviour
{
    [SerializeField] private OrbitWithData[] orbitsWithData;
    [SerializeField] private float mousePickRadius = 1.0f;
    [SerializeField] private LayerMask orbitLayerMask;

    RaycastHit[] hits = null;

    public IEnumerable<Orbit> GetUnlockedOrbits() => orbitsWithData.Where(x => x.Unlocked).Select(x => x.Orbit);
    public IEnumerable<Orbit> GetLockedOrbits() => orbitsWithData.Where(x => x.Unlocked).Select(x => x.Orbit);
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
        return MousePicking.PickObjectFromLayer(IsOrbitUnlocked, orbitLayerMask, out orbit, out hitPosition);
    }

    private OrbitWithData GetMatchingOrbitData(Orbit orbit) => orbitsWithData.First(x => x.Orbit == orbit);

    private void Start()
    {
        foreach (var orbit in GetUnlockedOrbits())
        {
            orbit.State = Orbit.VisualState.Visible;
        }
    }

    [System.Serializable] private class OrbitWithData
    {
        public Orbit Orbit;
        public bool Unlocked;
        public int Price;
    }
}
