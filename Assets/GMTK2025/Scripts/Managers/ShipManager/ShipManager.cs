using System.Collections.Generic;
using Reflex.Attributes;
using UnityEngine;

public class ShipManager
{
    [Inject]
    private ShipManagerConfig config;

    [Inject]
    private OrbitController orbitController;

    public List<ShipData> CurrentShipPool = new();

    [Inject]
    public void Initialize()
    {
        CurrentShipPool.AddRange(config.BaseShipPool);
    }

    public void SpawnShip(ShipData ship, Orbit preferredOrbit)
    {
        var spawnedShip = Object.Instantiate(
            config.ShipPrefab,
            Random.onUnitSphere * 80f,
            Quaternion.identity
        );
        spawnedShip.InitializeWith(ship, preferredOrbit);
        if (ship.Temporary && spawnedShip.TryGetComponent(out LeaveTimer timer))
        {
            timer.SetTimer(60.0f);
        }
    }

    public (ShipData, Orbit)[] DrawShips(int shipsToDraw)
    {
        (ShipData, Orbit)[] newShips = new (ShipData, Orbit)[shipsToDraw];

        Debug.Log(CurrentShipPool.Count);

        if (CurrentShipPool.Count < shipsToDraw)
        {
            Initialize();
        }

        Shuffle(CurrentShipPool);

        for (int i = 0; i < shipsToDraw; i++)
        {
            var shipData = CurrentShipPool[i];
            var orbit = SelectOrbitForShip(shipData);
            newShips[i] = (shipData, orbit);
        }

        return newShips;
    }

    private Orbit SelectOrbitForShip(ShipData shipdata)
    {
        // TODO: Select Orbit based in ship parameters
        var orbits = orbitController.GetUnlockedOrbits();
        return orbits.RandomElement();
    }

    public static void Shuffle<T>(IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
