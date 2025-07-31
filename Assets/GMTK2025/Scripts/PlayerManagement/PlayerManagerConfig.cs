using UnityEngine;

[CreateAssetMenu(
    menuName = "GMTK2025/Config/PlayerManagerConfig",
    fileName = "PlayerManagerConfig"
)]
public class PlayerManagerConfig : ScriptableObject
{
    [field: SerializeField]
    public ShipData[] BaseShipPool { get; private set; }

    [field: SerializeField]
    public Ship ShipPrefab { get; private set; }
}
