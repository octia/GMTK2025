using EditorAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "GMTK2025/Data/ShipData", fileName = "ShipData")]
public class ShipData : ScriptableObject
{
    public string Size = "WIP";
    public string Profit = "WIP";
    public string Speed = "WIP";
    public string Direction = "WIP";
    public bool Temporary;

    [Space]
    [AssetPreview]
    public Sprite Icon;
}
