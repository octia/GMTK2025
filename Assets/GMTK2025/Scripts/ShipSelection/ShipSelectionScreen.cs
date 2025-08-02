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
    [SerializeField] private CameraController shipSelectionCam;

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

    [Inject]
    private UIManager uiManager;

    [Inject]
    private PlayerDialogueController dialogueController;

    private bool hasTutorialPlayed = false;

    // ...
    private float currentNewShipTime;

    public (ShipData data, Orbit orbit)[] NewShips;

    // Initial
    private Animator anim;

    private Sprite defaultPilotPreview;

    private CameraController mainCam;

    private void Start()
    {
        currentNewShipTime = newShipTime;

        anim = GetComponent<Animator>();

        defaultPilotPreview = pilotPreview.sprite;

        mainCam = Camera.main.GetComponent<CameraController>();
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
        NewShips = playerManager.DrawShips(3);

        for (int i = 0; i < 3; i++)
        {
            selectionOptions[i].ChangeSelectionShip(NewShips[i].data);
        }

        soundManager.PausedGameVolume(true);
        SetScreenActive(true);

        if (!hasTutorialPlayed)
        {
            hasTutorialPlayed = true;
            dialogueController.PlayTextChain(PlayerDialogueChainType.NewShipTutorial);
        }
    }

    public void SetScreenActive(bool value)
    {
        OnScreen = value;

        if (value)
        {
            ShowScreen();
            anim.Play("Enter");

            ResetDialogPanel();

            shipSelectionCam.CurrentPosition = mainCam.CurrentPosition;
            shipSelectionCam.CurrentDistanceFromPlanet = mainCam.CurrentDistanceFromPlanet;
            shipSelectionCam.DesiredDistanceFromPlanet = mainCam.DesiredDistanceFromPlanet;

            uiManager.UICamera = shipSelectionCam.GetComponent<Camera>();
        }
        else
        {
            anim.Play("Exit");

            shipSelectionCam.CurrentPosition = mainCam.CurrentPosition;
            shipSelectionCam.CurrentDistanceFromPlanet = mainCam.CurrentDistanceFromPlanet;
            shipSelectionCam.DesiredDistanceFromPlanet = mainCam.DesiredDistanceFromPlanet;

            uiManager.UICamera = null;
        }
    }

    public void ShowScreen()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(OnScreen);
        }

        stationUpgradesScreen.SetActive(!OnScreen);
        mainCam.LockCamera = OnScreen;
    }

    public void VisualizeButton()
    {
        background.SetActive(!background.activeSelf);
        mainScreen.SetActive(!mainScreen.activeSelf);
    }

    public void UpdateStatsPanel(int i)
    {
        if (!OnScreen)
        {
            return;
        }

        ShipData ship = NewShips[i].data;

        temporaryText.gameObject.SetActive(ship.Temporary);
        nameText.text = ship.name;
        orbitText.text = "Preferred Orbital: " + ship.Orbit;
        sizeText.text = "Size: " + ship.Size;
        profitText.text = "Profit: " + ship.Profit + "$";
        speedText.text = "Speed: " + ship.Speed;
        directionText.text = ship.Clockwise
            ? "Direction: Clockwise"
            : "Direction: Counter Clockwise";

        pilotPreview.sprite = ship.Captain.PilotPortrait;
        // TODO: Randomize pilot dialog
        pilotDialog.StartDialog(ship.Captain.PilotDialogs[0], ship.Captain.TypeTime);
    }

    public void ResetDialogPanel()
    {
        pilotPreview.sprite = defaultPilotPreview;
        pilotDialog.StartDialog("...", 0.05f);
    }

    public void SelectNewShip(int value)
    {
        ShipData ship = null;
        for (int i = 0; i < 3; i++)
        {
            if (i == value)
            {
                playerManager.SpawnShip(NewShips[i].data, NewShips[i].orbit);
                ship = NewShips[i].data;
                continue;
            }
        }

        currentNewShipTime = newShipTime;
        soundManager.PlayOneShot(soundManager.soundManagerConfig.sfxSelectShipPath);
        if(ship!=null)
        {
            soundManager.PlayOneShotWithStatus(ship.Captain.sfxPath, "Accept");
        }

        soundManager.PausedGameVolume(false);
        SetScreenActive(false);
    }
}
