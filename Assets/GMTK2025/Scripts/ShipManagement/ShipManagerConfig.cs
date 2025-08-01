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
}
