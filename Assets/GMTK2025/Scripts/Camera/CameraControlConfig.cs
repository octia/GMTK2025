using UnityEngine;

[CreateAssetMenu(
    fileName = "CameraControlConfig",
    menuName = "GMTK2025/Config/CameraControlConfig"
)]
public class CameraControlConfig : ScriptableObject
{
    [field: SerializeField]
    public float MaxVerticalAngle { get; private set; } = 85f;

    [field: SerializeField]
    [field: Range(10, 1000)]
    public float HorizontalScrollMultiplier { get; private set; } = 1f;

    [field: SerializeField]
    [field: Range(0, 720)]
    public float MaxVelocityDegPerSec { get; private set; } = 50f;

    [field: SerializeField]
    public float DragInVelocityPercentPerSec { get; private set; } = 15f;

    [field: SerializeField]
    public float MinVelocityForInertia { get; private set; } = 5f;

    [field: SerializeField]
    public float MinDistanceFromPlanet { get; private set; } = 30f;

    [field: SerializeField]
    public float MaxDistanceFromPlanet { get; private set; } = 120f;
}
