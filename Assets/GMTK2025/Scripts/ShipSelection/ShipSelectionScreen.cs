using Reflex.Attributes;
using UnityEngine;

public class ShipSelectionScreen : MonoBehaviour
{
    [Tooltip("Time necessary for a new ship to appear")]
    [SerializeField] private float newShipTime;

    [Inject]
    private PlayerManager playerManager;

    // ...
    private float currentNewShipTime;

    private bool onScreen;

    private void Update()
    {
        if (!onScreen)
        {
            currentNewShipTime -= Time.deltaTime;

            if (currentNewShipTime < 0)
            {
                StartSelection();
            }

            return;
        }

    }

    private void StartSelection()
    {
        ShipData[] newShips = playerManager.DrawShips();

        transform.GetChild(0).gameObject.SetActive(true);
    }
}
