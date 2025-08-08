using Reflex.Attributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShipSelectionScreen : MonoBehaviour
{
    [Tooltip("Time necessary for a new ship to appear")]
    [SerializeField]
    private float[] newShipTime;

    [Space]
    [SerializeField]
    private GameObject stationUpgradesScreen;

    [Space]
    [SerializeField]
    private ShipSelectionOption[] selectionOptions;

    [Space]
    [SerializeField]
    private CameraController shipSelectionCam;

    [SerializeField]
    private GameObject background;

    [SerializeField]
    private GameObject mainScreen;

    [Space]
    [SerializeField]
    private ShipStatsPanel shipStatsPanel;

    [Space]
    [SerializeField]
    private Image newShipTimeoutBar;

    [Space]
    [SerializeField]
    private TMP_Text newShipTimeText;

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

    [Inject]
    private CameraController mainCam;

    private bool hasTutorialPlayed = false;

    // ...
    private float currentNewShipTime;
    private float lastNewShipTime;

    private int newShipsAmount;

    public (ShipData data, Orbit orbit, float stayTime)[] NewShips;

    // Initial
    private Animator anim;

    private Sprite defaultPilotPreview;

    private void Start()
    {
        currentNewShipTime = newShipTime[0];
        lastNewShipTime = currentNewShipTime;

        anim = GetComponent<Animator>();

        defaultPilotPreview = pilotPreview.sprite;
    }

    private void Update()
    {
        if (!OnScreen)
        {
            currentNewShipTime -= Time.deltaTime * timerManager.TimeScale;

            if (newShipTimeText)
            {
                newShipTimeText.text = Mathf.Round(currentNewShipTime) + "s";
            }
            if (newShipTimeoutBar)
            {
                newShipTimeoutBar.rectTransform.localScale = new Vector3(Mathf.Clamp01(currentNewShipTime / lastNewShipTime), 1, 1);
            }

            if (currentNewShipTime < 0 && !Mouse.current.leftButton.isPressed || currentNewShipTime <= -2.0f)
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

            mainCam.SetCameraOffset(true);
        }
        else
        {
            anim.Play("Exit");

            mainCam.SetCameraOffset(false);
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
        if (!OnScreen)
        {
            return;
        }

        ShipData ship = NewShips[i].data;

        shipStatsPanel.SetData(ship, NewShips[i].stayTime);

        pilotPreview.sprite = ship.Captain.PilotPortrait;
        // TODO: Randomize pilot dialog
        pilotDialog.StartDialog(ship.Captain.PilotDialogs[0], ship.Captain.TypeTime);

        soundManager.PlayOneShot($"event:/Spaceship_preview/Ship_{ship.Captain.PilotName}_preview");
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

        newShipsAmount++;
        currentNewShipTime = newShipTime[
            UtilityFunctions.GetKeyOrLast(newShipsAmount, newShipTime.Length)
        ];
        lastNewShipTime = currentNewShipTime;
        soundManager.PlayOneShot(soundManager.soundManagerConfig.sfxSelectShipPath);
        if (ship != null)
        {
            soundManager.PlayOneShotWithStatus(ship.Captain.sfxPath, "Accept");
        }

        soundManager.PausedGameVolume(false);
        SetScreenActive(false);
    }
}
