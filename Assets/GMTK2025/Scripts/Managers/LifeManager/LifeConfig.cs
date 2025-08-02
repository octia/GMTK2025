using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LifeConfig", menuName = "GMTK2025/Config/LifeConfig")]
public class LifeConfig : ScriptableObject
{
    [field: Range(1, 99)]
    [field: SerializeField]
    public int StartLifeCount { get; private set; } = 3;

    [field: Range(1, 99)]
    [field: SerializeField]
    public int MaxLifeCount { get; private set; } = 3;
}
