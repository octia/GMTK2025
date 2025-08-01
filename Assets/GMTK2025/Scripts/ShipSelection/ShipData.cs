using EditorAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "GMTK2025/Data/ShipData", fileName = "ShipData")]
public class ShipData : ScriptableObject
{
    public string Orbit = "WIP";
    public string Size;
    public int Profit;
    public int Speed;
    public bool Clockwise;
    public bool Temporary;

    [Space]
    [AssetPreview]
    public Sprite Icon;
    
    [Space]
    public CaptainData Captain;
}
