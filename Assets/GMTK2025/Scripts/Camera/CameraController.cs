using Reflex.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private new Transform camera;

    [SerializeField]
    private Transform origin;

    [Inject]
    private CameraControlConfig config;

    [SerializeField]
    private Vector2 currentPosition = new(0f, 1f);

    private float currentDistanceFromPlanet = 50f;

    private void Start() { }

    private void Update()
    {
        if (Mouse.current.rightButton.isPressed)
        {
            var mouseDelta = Mouse.current.delta.value;
            mouseDelta.y *= -1f;
            currentPosition += mouseDelta;
        }

        currentPosition.y = Mathf.Clamp(
            currentPosition.y,
            -config.MaxVerticalAngle,
            config.MaxVerticalAngle
        );
        if (currentPosition.x > 360f)
        {
            currentPosition.x -= 360f;
        }
        SetCameraPosition(currentPosition, currentDistanceFromPlanet);
    }

    private void SetCameraPosition(Vector2 degreePosition, float distance)
    {
        Vector3 direction = -Vector3.forward;
        Quaternion currentRotation = Quaternion.Euler(0f, degreePosition.x, 0f);
        direction = Quaternion.Euler(degreePosition.y, 0f, 0f) * direction;
        direction = currentRotation * direction;
        camera.position = origin.position + direction * distance;
        camera.LookAt(origin, Vector3.up);
    }
}
