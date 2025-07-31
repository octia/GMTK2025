using Reflex.Attributes;
using TMPro;
using UnityEngine;

public class ShipSelectionScreen : MonoBehaviour
{
    [Tooltip("Time necessary for a new ship to appear")]
    [SerializeField]
    private float newShipTime;

    [Space]
    [SerializeField]
    private ShipSelectionOption[] selectionOptions;

    [Space]
    [SerializeField]
    private GameObject background;

    [SerializeField]
    private GameObject mainScreen;

    [Space]
    [SerializeField]
    private TMP_Text newShipTimeText;

    [Space]
    [SerializeField]
    private TMP_Text nameText;

    [SerializeField]
    private TMP_Text temporaryText;

    [SerializeField]
    private TMP_Text orbitText;

    [SerializeField]
    private TMP_Text sizeText;

    [SerializeField]
    private TMP_Text profitText;

    [SerializeField]
    private TMP_Text speedText;

    [SerializeField]
    private TMP_Text directionText;

    [Inject]
    private PlayerManager playerManager;

    // ...
    private float currentNewShipTime;

    private bool onScreen;

    private ShipData[] newShips;

    private void Start()
    {
        currentNewShipTime = newShipTime;
    }

    private void Update()
    {
        if (!onScreen)
        {
            currentNewShipTime -= Time.deltaTime;
            //newShipTimeText.text = Math.Round(currentNewShipTime, 2) + "s";

            if (currentNewShipTime < 0)
            {
                StartSelection();
            }

            return;
        }
    }

    private void StartSelection()
    {
        newShips = playerManager.DrawShips();

        for (int i = 0; i < 3; i++)
        {
            selectionOptions[i].ChangeSelectionShip(newShips[i]);
        }

        SetScreenActive(true);

        onScreen = true;
    }

    public void SetScreenActive(bool value)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(value);
        }
    }

    public void VisualizeButton()
    {
        background.SetActive(!background.activeSelf);
        mainScreen.SetActive(!mainScreen.activeSelf);
    }

    public void UpdateStatsPanel(int i)
    {
        ShipData ship = newShips[i];

        temporaryText.gameObject.SetActive(ship.Temporary);
        nameText.text = ship.name;
        orbitText.text = "Preferred Orbital: " + ship.Orbit;
        sizeText.text = "Size: " + ship.Size;
        profitText.text = "Profit: " + ship.Profit + "$";
        speedText.text = "Speed: " + ship.Speed;
        directionText.text = ship.Clockwise
            ? "Direction: Clockwise"
            : "Direction: Counter Clockwise";
    }

    public void SelectNewShip(int value)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == value)
            {
                // TODO: Put selected ship on the planet
                continue;
            }

            playerManager.CurrentShipPool.Add(newShips[i]);
        }

        currentNewShipTime = newShipTime;

        onScreen = false;
        SetScreenActive(false);
    }
}
