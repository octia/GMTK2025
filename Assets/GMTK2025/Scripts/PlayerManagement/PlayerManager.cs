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

    public void SpawnShip(ShipData ship) { }

    public ShipData[] DrawShips()
    {
        ShipData[] newShips = new ShipData[3];

        Debug.Log(CurrentShipPool.Count);

        for (int i = 0; i < 3; i++)
        {
            int n;

            n = Random.Range(0, CurrentShipPool.Count);

            newShips[i] = CurrentShipPool[n];

            CurrentShipPool.RemoveAt(n);
        }

        return newShips;
    }
}
