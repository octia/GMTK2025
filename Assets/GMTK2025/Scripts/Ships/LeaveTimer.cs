using EditorAttributes;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class LeaveTimer : MonoBehaviour
{
    private Ship ship;

    [ShowInInspector] private float initialTime = 0;
    [ShowInInspector] private float timeLeft = 0;
    [SerializeField] private BarIndicator barIndicator;

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

        timeLeft -= Time.fixedDeltaTime;
        if (timeLeft <= 0)
        {
            timeLeft = 0;
            ship.ChangeOrbit(null);
        }
        if (barIndicator)
        {
            barIndicator.SetRatio(timeLeft / initialTime);
        }
    }
}
