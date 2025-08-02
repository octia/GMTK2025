using UnityEngine;

[CreateAssetMenu(
    menuName = "GMTK2025/Config/UpgradeManagerConfig",
    fileName = "UpgradeManagerConfig"
)]
public class UpgradeManagerConfig : ScriptableObject
{
    [SerializeField]
    public int[] UpgradeCostPerPurchase;
}
