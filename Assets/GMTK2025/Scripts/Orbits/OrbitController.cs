using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrbitController : MonoBehaviour
{
    [SerializeField] private OrbitWithData[] orbitsWithData;

    public IEnumerable<Orbit> GetUnlockedOrbits() => orbitsWithData.Where(x => x.Unlocked).Select(x => x.Orbit);
    public IEnumerable<Orbit> GetLockedOrbits() => orbitsWithData.Where(x => x.Unlocked).Select(x => x.Orbit);
    public int GetOrbitPrice(Orbit orbit) => GetMatchingOrbitData(orbit).Price;
    public bool GetOrbitUnlockStatus(Orbit orbit) => GetMatchingOrbitData(orbit).Unlocked;
    public void UnlockOrbit(Orbit orbit)
    {
        var orbitWithData = GetMatchingOrbitData(orbit);
        orbitWithData.Unlocked = true;
        orbitWithData.Orbit.State = Orbit.VisualState.Visible;
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
