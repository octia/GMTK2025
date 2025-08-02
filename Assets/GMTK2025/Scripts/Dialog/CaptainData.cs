using EditorAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "GMTK2025/Data/CaptainData", fileName = "CaptainData")]
public class CaptainData : ScriptableObject
{
    [AssetPreview]
    public Sprite PilotPortrait;
    
    public float TypeTime;
    public string sfxPath;
    public string PilotName;
    [TextArea]
    public string[] PilotDialogs;
}
