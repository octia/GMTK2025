using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerDialogueChain",
    menuName = "GMTK2025/Config/PlayerDialogueChain"
)]
public class PlayerDialogueChain : ScriptableObject
{
    [field: SerializeField]
    public PlayerDialogueChainType ChainType { get; private set; }

    [field: SerializeField]
    public List<PlayerDialogue> Dialogues { get; private set; }
}
