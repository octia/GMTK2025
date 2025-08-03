using EditorAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "GMTK2025/Data/UpgradeData", fileName = "UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public UpgradeType UpgradeType;

    public string[] Name;

    [Tooltip("Description might change per level of upgrade")]
    [TextArea]
    public string[] Descriptions;

    public int[] Cost;

    [AssetPreview]
    public Sprite[] Icons;
}
