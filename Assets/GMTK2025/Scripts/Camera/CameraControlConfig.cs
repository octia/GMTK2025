using UnityEngine;

[CreateAssetMenu(
    fileName = "CameraControlConfig",
    menuName = "GMTK2025/Config/CameraControlConfig"
)]
public class CameraControlConfig : ScriptableObject
{
    [field: SerializeField]
    [field: Range(0f, 89)]
    public float MaxVerticalAngle { get; private set; } = 85f;

    [field: SerializeField]
    [field: Range(10, 1000)]
    public float HorizontalScrollMultiplier { get; private set; } = 1f;

    [field: SerializeField]
    [field: Range(0, 7200)]
    public float MaxVelocityDegPerSec { get; private set; } = 50f;

    [field: SerializeField]
    [field: Range(0, 60)]
    public float HorizontalDrag { get; private set; } = 15f;

    [field: SerializeField]
    [field: Range(0, 60)]
    public float VerticalDrag { get; internal set; } = 10f;

    [field: SerializeField]
    [field: Range(0, 7200)]
    public float MinVelocityForInertia { get; private set; } = 5f;

    [field: SerializeField]
    [field: Range(0, 100)]
    public float MinDistanceFromPlanet { get; private set; } = 30f;

    [field: SerializeField]
    [field: Range(0, 500)]
    public float MaxDistanceFromPlanet { get; private set; } = 120f;

    [field: SerializeField]
    public Vector2 DragSensitivity { get; private set; } = Vector2.one;

    [field: SerializeField]
    [field: Range(0f, 1f)]
    public float InertiaCalculationDuration { get; private set; } = 0.16f;

    [field: SerializeField]
    [field: Range(0f, 20f)]
    public float ScrollSensitivity { get; internal set; } = 3f;

    [field: SerializeField]
    [field: Range(0.01f, 1f)]
    public float ScrollSnapDuration { get; internal set; } = 0.1f;
}
