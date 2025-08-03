using PrimeTween;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private new Transform camera;

    [SerializeField]
    private Transform realCameraForOffset;

    [SerializeField]
    private Transform origin;

    [SerializeField]
    private bool useInertia;

    [Inject]
    private CameraControlConfig config;

    [Inject]
    private SoundManager soundManager;

    [Inject]
    private PlayerDialogueController playerDialogueController;

    public Vector2 CurrentPosition = new(0f, 60f);

    [SerializeField]
    private Vector2 currentVelocity = new(0f, 0f);

    [SerializeField]
    private Vector3 minOffsetForUISelection = Vector3.right;

    [SerializeField]
    private Vector3 maxOffsetForUISelection = Vector3.right;

    [HideInInspector]
    public float CurrentDistanceFromPlanet = 50f;

    [HideInInspector]
    public float DesiredDistanceFromPlanet = 50f;

    private float normalizedOffset = 0;

    private float normalizedOffsetUse = 0;

    private bool hasOffset = false;
    private float inertiaTime = 0;
    private static bool hasPuked = false;

    private Tween scrollTween;

    private float normalizedDistance = 0f;

    private void Awake()
    {
        CurrentDistanceFromPlanet = DesiredDistanceFromPlanet;
        normalizedDistance = GetNormalizedDistance(CurrentDistanceFromPlanet);
        soundManager.UpdateMusicDistance(normalizedDistance);
    }

    private void Update()
    {
        HandleMovmenent();
        if (!Mathf.Approximately(Mouse.current.scroll.value.y, 0))
        {
            HandleScroll();
        }

        SetCameraPosition(CurrentPosition, CurrentDistanceFromPlanet);

        if (currentVelocity.sqrMagnitude > 25)
        {
            inertiaTime += Time.deltaTime;
        }
        else
        {
            inertiaTime = 0;
        }

        if (inertiaTime > 15 && !hasPuked)
        {
            hasPuked = true;
            playerDialogueController.PlayTextChain(PlayerDialogueChainType.Eeeeugh);
        }
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
            (value) =>
            {
                CurrentDistanceFromPlanet = value;
                normalizedDistance = GetNormalizedDistance(value);
                SetOffset();
            }
        );

        //Update music distance
        soundManager.UpdateMusicDistance(normalizedDistance);
    }

    private void SetOffset()
    {
        float normalizedOffset = normalizedDistance;
        float offsetToUse = normalizedDistance * normalizedOffset;
        var realOffset = Vector3.Lerp(
            minOffsetForUISelection,
            maxOffsetForUISelection,
            normalizedDistance
        );
        realOffset *= normalizedOffsetUse;
        realCameraForOffset.localPosition = realOffset;
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

    public void SetCameraOffset(bool shouldHaveOffset)
    {
        hasOffset = shouldHaveOffset;
        float startOffset = normalizedOffsetUse;
        float endOffset = shouldHaveOffset ? 1 : 0;
        var tween = Tween.Custom(
            startOffset,
            endOffset,
            0.4f,
            (newValue) => normalizedOffsetUse = newValue
        );
        tween.OnUpdate(this, (_, _) => SetOffset());
    }

    private float GetNormalizedDistance(float distance)
    {
        return Mathf.InverseLerp(
            config.MinDistanceFromPlanet,
            config.MaxDistanceFromPlanet,
            distance
        );
    }
}
