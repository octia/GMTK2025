using PrimeTween;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public bool LockCamera;

    [SerializeField]
    private new Transform camera;

    [SerializeField]
    private Transform origin;

    [SerializeField]
    private bool useInertia;

    [Inject]
    private CameraControlConfig config;

    [Inject]
    private SoundManager soundManager;

    public Vector2 CurrentPosition = new(0f, 60f);

    [SerializeField]
    private Vector2 currentVelocity = new(0f, 0f);

    [HideInInspector]
    public float CurrentDistanceFromPlanet = 50f;

    [HideInInspector]
    public float DesiredDistanceFromPlanet = 50f;

    private Tween scrollTween;

    private void Awake()
    {
        CurrentDistanceFromPlanet = DesiredDistanceFromPlanet;
    }

    private void Update()
    {
        if (LockCamera)
        {
            return;
        }

        HandleMovmenent();
        HandleScroll();

        SetCameraPosition(CurrentPosition, CurrentDistanceFromPlanet);
    }

    private void HandleScroll()
    {
        var mouseScroll = Mouse.current.scroll.value.y;
        if (Mathf.Approximately(mouseScroll, 0f))
        {
            return;
        }

        DesiredDistanceFromPlanet += -mouseScroll * config.ScrollSensitivity;
        DesiredDistanceFromPlanet = Mathf.Clamp(
            DesiredDistanceFromPlanet,
            config.MinDistanceFromPlanet,
            config.MaxDistanceFromPlanet
        );

        if (scrollTween.isAlive)
        {
            scrollTween.Stop();
        }
        scrollTween = Tween.Custom(
            CurrentDistanceFromPlanet,
            DesiredDistanceFromPlanet,
            config.ScrollSnapDuration,
            (value) => CurrentDistanceFromPlanet = value
        );

        //Update music distance
        soundManager.UpdateMusicDistance(CurrentDistanceFromPlanet, config.MaxDistanceFromPlanet);
    }

    private void HandleMovmenent()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            currentVelocity = Vector2.zero;
        }

        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            if (
                currentVelocity.sqrMagnitude
                < config.MinVelocityForInertia * config.MinVelocityForInertia
            )
            {
                currentVelocity = Vector2.zero;
            }
        }

        if (Mouse.current.rightButton.isPressed)
        {
            var mouseDelta = Vector2.Scale(Mouse.current.delta.value, config.DragSensitivity);
            mouseDelta.y *= -1f;

            CurrentPosition.y = Mathf.Clamp(
                CurrentPosition.y,
                -config.MaxVerticalAngle,
                config.MaxVerticalAngle
            );
            ClampPosition();

            CurrentPosition += mouseDelta;

            int targetFrameCountPerSecond = Mathf.Max(
                Application.targetFrameRate,
                Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value)
            );
            int pastFramesToLookAt = Mathf.CeilToInt(
                config.InertiaCalculationDuration * targetFrameCountPerSecond
            );

            pastFramesToLookAt = Mathf.Max(2, pastFramesToLookAt);

            currentVelocity =
                currentVelocity * (pastFramesToLookAt - 1f) / pastFramesToLookAt
                + (mouseDelta / Time.deltaTime / pastFramesToLookAt);
        }
        else
        {
            if (useInertia)
            {
                CurrentPosition += currentVelocity * Time.deltaTime;
            }

            float horizontalVelocity = Mathf.MoveTowards(
                currentVelocity.x,
                0f,
                Mathf.Abs(currentVelocity.x) * config.HorizontalDrag * Time.deltaTime
            );
            float verticalVelocity = Mathf.MoveTowards(
                currentVelocity.y,
                0f,
                Mathf.Abs(currentVelocity.y) * config.VerticalDrag * Time.deltaTime
            );

            currentVelocity = new Vector2(horizontalVelocity, verticalVelocity);

            if (
                currentVelocity.sqrMagnitude
                > config.MaxVelocityDegPerSec * config.MaxVelocityDegPerSec
            )
            {
                currentVelocity = currentVelocity.normalized * config.MaxVelocityDegPerSec;
            }

            if (currentVelocity.sqrMagnitude < 1)
            {
                currentVelocity = Vector2.zero;
            }
        }
    }

    private void ClampPosition()
    {
        if (CurrentPosition.x > 360f)
        {
            CurrentPosition.x -= 360f;
        }

        if (CurrentPosition.x < 0f)
        {
            CurrentPosition.x += 360f;
        }
    }

    public void SetCameraPosition(Vector2 degreePosition, float distance)
    {
        Vector3 direction = -Vector3.forward;
        Quaternion currentRotation = Quaternion.Euler(0f, degreePosition.x, 0f);
        direction = Quaternion.Euler(degreePosition.y, 0f, 0f) * direction;
        direction = currentRotation * direction;
        camera.position = origin.position + direction * distance;
        camera.LookAt(origin, Vector3.up);
    }
}
