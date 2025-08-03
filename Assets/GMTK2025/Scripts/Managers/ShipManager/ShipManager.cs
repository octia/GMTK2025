using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
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

    public void SpawnShip(ShipData ship, Orbit preferredOrbit, float leaveTime = 60)
    {
        Vector3 randomPlanarStart =
            Quaternion.Euler(0, Random.value * 360, 0) * Vector3.forward * 1000;

        preferredOrbit.GetNearestPoint(
            randomPlanarStart,
            out Vector3 nearestPoint,
            out _,
            out float speed
        );

        Vector3 planarToOrbitDirection = nearestPoint - randomPlanarStart;
        planarToOrbitDirection.y = 0;
        planarToOrbitDirection.Normalize();

        Vector3 spawnOffsetDirection = (-planarToOrbitDirection * 2 + Vector3.up * 1).normalized;
        Vector3 spawnPosition =
            nearestPoint
            + config.SpawnToOrbitJoinTime
                * config.SpeedTypeToFloat[(int)ship.Speed]
                * speed
                * spawnOffsetDirection;
        var spawnedShip = Object.Instantiate(
            config.ShipPrefab,
            spawnPosition,
            Quaternion.LookRotation(-spawnOffsetDirection, Vector3.up)
        );
        spawnedShip.InitializeWith(ship, preferredOrbit);
        if (ship.Temporary && spawnedShip.TryGetComponent(out LeaveTimer timer))
        {
            timer.SetTimer(leaveTime);
        }
    }

    public (ShipData, Orbit, float)[] DrawShips(int shipsToDraw)
    {
        (ShipData, Orbit, float)[] newShips = new (ShipData, Orbit, float)[shipsToDraw];

        Debug.Log(CurrentShipPool.Count);

        if (CurrentShipPool.Count < shipsToDraw)
        {
            Initialize();
        }

        List<Orbit> orbits = new(shipsToDraw);

        for (int i = 0; i < shipsToDraw; i++)
        {
            var shipData = CurrentShipPool.RandomElement();
            var orbit = SelectOrbitForShip(shipData, orbits);
            orbits.Add(orbit);
            newShips[i] = (shipData, orbit, shipData.Temporary ? 50 + 10 * Random.Range(0, 4) : 0);
        }

        return newShips;
    }

    private Orbit SelectOrbitForShip(ShipData shipdata, List<Orbit> differentFrom)
    {
        var avaliableOrbits = orbitController.GetUnlockedOrbits().ToList();

        for (int i = 0; i < avaliableOrbits.Count; i++)
        {
            Orbit orbit = avaliableOrbits[i];
            if (differentFrom.Contains(orbit))
            {
                avaliableOrbits.Remove(orbit);
                i--;
                continue;
            }

            OrbitShape orbitShape =
                orbit.Generator.Eccentricity <= 0.01f ? OrbitShape.circular : OrbitShape.elliptical;
            if (shipdata.PreferredOrbitShape != orbitShape)
            {
                avaliableOrbits.Remove(orbit);
                i--;
            }
        }

        var toReturn =
            avaliableOrbits.Count > 0
                ? avaliableOrbits.RandomElement()
                : orbitController.GetUnlockedOrbits().RandomElement();

        return toReturn;
    }
}
