using EditorAttributes;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class LeaveTimer : MonoBehaviour
{
    private Ship ship;

    [SerializeField] private float startTime = 0;
    [ShowInInspector] private float timeLeft = 0;

    private void Awake()
    {
        ship = GetComponent<Ship>();
        timeLeft = startTime;
    }

    [Button]
    public void Init(float time)
    {
        startTime = time;
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
            ship.ChangeOrbit(null);
        }
    }
}
