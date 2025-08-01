using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerDialogueConfig",
    menuName = "GMTK2025/Config/PlayerDialogueConfig"
)]
public class PlayerDialogueConfig : ScriptableObject
{
    [field: SerializeField]
    public List<PlayerDialogueChain> PlayerDialogueChains { get; private set; }

    [field: SerializeField]
    [field: Range(1, 200)]
    public float CharactersPerSecond { get; private set; } = 20f;

    [field: SerializeField]
    public Sprite DefaultIcon { get; private set; }
}
