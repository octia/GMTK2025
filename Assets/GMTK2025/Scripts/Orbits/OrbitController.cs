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
    /// Tries to pick an unlocked orbit with the mouse.
    /// </summary>
    /// <returns>Returns a close orbit to the mouse, or null</returns>
    public Orbit PickUnlockedOrbitWithMouse()
    {
        hits ??= new RaycastHit[orbitsWithData.Length];

        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);

        for(int attempt = 0; attempt <= 3; attempt++)
        {
            Orbit closestOrbit = null;
            float closestDistance = Mathf.Infinity;
            var hitCount = Physics.SphereCastNonAlloc(ray, mousePickRadius * (attempt / 3.0f), hits, Mathf.Infinity, orbitLayerMask);
            for (int i = 0; i < hitCount; i++)
            {
                if (hits[i].distance >= closestDistance)
                {
                    continue;
                }
                var orbit = hits[i].transform.GetComponent<Orbit>();
                if (!IsOrbitUnlocked(orbit))
                {
                    continue;
                }
                closestOrbit = orbit;
                closestDistance = hits[i].distance;
            }
            if (closestOrbit)
            {
                return closestOrbit;
            }
        }
        return null;
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
