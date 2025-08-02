using UnityEngine;

[CreateAssetMenu(menuName = "GMTK2025/Data/ShipData", fileName = "ShipData")]
public class ShipData : ScriptableObject
{
    public OrbitShape PreferredOrbitShape;
    public int Profit;
    public SpeedType Speed;
    public bool Clockwise;
    public bool Temporary;
    public GameObject ModelPrefab;
    
    [Space]
    public CaptainData Captain;
}
