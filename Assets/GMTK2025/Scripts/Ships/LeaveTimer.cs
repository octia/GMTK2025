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

    [ShowInInspector]
    private float initialTime = 0;

    [ShowInInspector]
    private float timeLeft = 0;

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
        initialTime = time;
        timeLeft = time;
    }

    private void FixedUpdate()
    {
        if (timeLeft <= 0)
        {
            return;
        }

        timeLeft -= Time.fixedDeltaTime * timerManager.TimeScale;
        if (timeLeft <= 0)
        {
            timeLeft = 0;
            ship.ChangeOrbit(null);
            destroySequence = Sequence
                .Create()
                .ChainDelay(12.0f)
                .Chain(Tween.Scale(transform, 0, 3.0f))
                .OnComplete(() => Destroy(gameObject));
        }
        if (barIndicator)
        {
            barIndicator.SetRatio(timeLeft / initialTime);
        }
    }

    private void OnDestroy()
    {
        destroySequence.Stop();
    }
}
