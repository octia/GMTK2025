using UnityEngine;

[CreateAssetMenu(
    menuName = "GMTK2025/Config/ShipCollisionManagerConfig",
    fileName = "ShipCollisionManagerConfig"
)]
public class ShipCollisionManagerConfig : ScriptableObject
{
    [SerializeField]
    public GameObject explosionPrefab;
}
