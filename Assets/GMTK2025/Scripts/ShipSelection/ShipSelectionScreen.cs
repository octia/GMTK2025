using System.IO;
using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelectionScreen : MonoBehaviour
{
    [Tooltip("Time necessary for a new ship to appear")]
    [SerializeField]
    private float newShipTime;

    [Space]
    [SerializeField]
    private GameObject stationUpgradesScreen;

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

    [Space]
    [SerializeField]
    private Image pilotPreview;

    [SerializeField]
    private DialogController pilotDialog;

    [HideInInspector]
    public bool OnScreen;

    [Inject]
    private ShipManager playerManager;

    [Inject]
    private TimerManager timerManager;

    [Inject]
    private SoundManager soundManager;

    // ...
    private float currentNewShipTime;

    private (ShipData data, Orbit orbit)[] newShips;

    // Initial
    private Animator anim;

    private Sprite defaultPilotPreview;

    private void Start()
    {
        currentNewShipTime = newShipTime;

        anim = GetComponent<Animator>();

        defaultPilotPreview = pilotPreview.sprite;
    }

    private void Update()
    {
        if (!OnScreen)
        {
            currentNewShipTime -= Time.deltaTime * timerManager.TimeScale;
            // Debug
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
        newShips = playerManager.DrawShips(3);

        for (int i = 0; i < 3; i++)
        {
            selectionOptions[i].ChangeSelectionShip(newShips[i].data);
        }

        SetScreenActive(true);
    }

    public void SetScreenActive(bool value)
    {
        OnScreen = value;

        if (value)
        {
            ShowScreen();
            anim.Play("Enter");
        }
        else
        {
            anim.Play("Exit");
        }
    }

    public void ShowScreen()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(OnScreen);
        }

        stationUpgradesScreen.SetActive(!OnScreen);
    }

    public void VisualizeButton()
    {
        background.SetActive(!background.activeSelf);
        mainScreen.SetActive(!mainScreen.activeSelf);
    }

    public void UpdateStatsPanel(int i)
    {
        ShipData ship = newShips[i].data;

        temporaryText.gameObject.SetActive(ship.Temporary);
        nameText.text = ship.name;
        orbitText.text = "Preferred Orbital: " + ship.Orbit;
        sizeText.text = "Size: " + ship.Size;
        profitText.text = "Profit: " + ship.Profit + "$";
        speedText.text = "Speed: " + ship.Speed;
        directionText.text = ship.Clockwise
            ? "Direction: Clockwise"
            : "Direction: Counter Clockwise";

        pilotPreview.sprite = ship.PilotPreview;
        pilotDialog.StartDialog(ship.PilotDialog, ship.DialogTypeTime);
    }

    public void ResetDialogPanel()
    {
        pilotPreview.sprite = defaultPilotPreview;
        pilotDialog.StartDialog("* ...", 0.05f);
    }

    public void SelectNewShip(int value)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == value)
            {
                playerManager.SpawnShip(newShips[i].data, newShips[i].orbit);
                continue;
            }
        }

        currentNewShipTime = newShipTime;
        soundManager.PlayOneShot(soundManager.soundManagerConfig.sfxSelectShipPath);
        SetScreenActive(false);
    }
}
