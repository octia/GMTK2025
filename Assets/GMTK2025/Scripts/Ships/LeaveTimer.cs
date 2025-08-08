using EditorAttributes;
using PrimeTween;
using Reflex.Attributes;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class LeaveTimer : MonoBehaviour
{
    private Ship ship;

    [Inject]
    TimerManager timerManager;

    [field: ShowInInspector] public float InitialTime { get; private set; } = 0;

    [field: ShowInInspector] public float TimeLeft { get; private set; } = 0;

    [SerializeField]
    private BarIndicator barIndicator;
    Sequence destroySequence;

    private void Awake()
    {
        ship = GetComponent<Ship>();
    }

    [Button]
    public void SetTimer(float time)
    {
        InitialTime = time;
        TimeLeft = time;
    }

    private void FixedUpdate()
    {
        if (TimeLeft <= 0)
        {
            return;
        }

        TimeLeft -= Time.fixedDeltaTime * timerManager.TimeScale;
        if (TimeLeft <= 0)
        {
            TimeLeft = 0;
            ship.ChangeOrbit(null);
            destroySequence = Sequence
                .Create()
                .ChainDelay(12.0f)
                .Chain(Tween.Scale(transform, 0, 3.0f))
                .OnComplete(() => Destroy(gameObject));
        }
        if (barIndicator)
        {
            barIndicator.SetRatio(TimeLeft / InitialTime);
        }
    }

    private void OnDestroy()
    {
        destroySequence.Stop();
    }
}
