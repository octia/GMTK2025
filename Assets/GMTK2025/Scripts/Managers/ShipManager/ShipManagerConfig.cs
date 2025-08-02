using UnityEngine;

[CreateAssetMenu(
    menuName = "GMTK2025/Config/ShipManagerConfig",
    fileName = "ShipManagerConfig"
)]
public class ShipManagerConfig : ScriptableObject
{
    [field: SerializeField]
    public ShipData[] BaseShipPool { get; private set; }

    [field: SerializeField]
    public Ship ShipPrefab { get; private set; }

    [field: SerializeField]
    public float[] SpeedTypeToFloat = { 12, 22, 32 };

    [field: SerializeField]
    public float SpawnToOrbitJoinTime = 8;
}
