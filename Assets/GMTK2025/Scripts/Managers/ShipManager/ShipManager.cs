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
        Vector3 randomPlanarStart = Quaternion.Euler(0, Random.value * 360, 0) * Vector3.forward * 1000;

        preferredOrbit.GetNearestPoint(randomPlanarStart, out Vector3 nearestPoint, out _, out float speed);

        Vector3 planarToOrbitDirection = nearestPoint - randomPlanarStart;
        planarToOrbitDirection.y = 0;
        planarToOrbitDirection.Normalize();

        Vector3 spawnOffsetDirection = (-planarToOrbitDirection * 2 + Vector3.up * 1).normalized;
        Vector3 spawnPosition = nearestPoint
            + config.SpawnToOrbitJoinTime * config.SpeedTypeToFloat[(int)ship.Speed] * speed * spawnOffsetDirection;
        var spawnedShip = Object.Instantiate(
            config.ShipPrefab,
            spawnPosition,
            Quaternion.LookRotation(-spawnOffsetDirection, Vector3.up)
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
