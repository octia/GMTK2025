using System.Collections.Generic;
using Reflex.Attributes;
using UnityEngine;

public class PlayerManager
{
    [Inject]
    private PlayerManagerConfig config;

    public List<ShipData> CurrentShipPool = new();

    [Inject]
    public void Initialize()
    {
        CurrentShipPool.AddRange(config.BaseShipPool);
    }

    public void SpawnShip(ShipData ship)
    {
        var spawnedShip = Object.Instantiate(
            config.ShipPrefab,
            Random.onUnitSphere * 80f,
            Quaternion.identity
        );
        spawnedShip.InitializeWith(ship);
    }

    public ShipData[] DrawShips(int shipsToDraw)
    {
        ShipData[] newShips = new ShipData[shipsToDraw];

        Debug.Log(CurrentShipPool.Count);

        if (CurrentShipPool.Count < shipsToDraw)
        {
            Initialize();
        }

        for (int i = 0; i < shipsToDraw; i++)
        {
            int n;

            n = Random.Range(0, CurrentShipPool.Count);

            newShips[i] = CurrentShipPool[n];

            CurrentShipPool.RemoveAt(n);
        }

        return newShips;
    }
}
