using System;
using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TimeSlowAbility : MonoBehaviour
{
    [SerializeField]
    private float[] activeTime;

    [SerializeField]
    private float[] abilityCooldown;

    [Space]
    [SerializeField]
    private Button button;

    [SerializeField]
    private Scrollbar cooldownBar;

    [SerializeField]
    private Scrollbar activeBar;

    [SerializeField]
    private TMP_Text abilityCooldownText;

    [SerializeField]
    private Color disabledColor;

    [Inject]
    private TimerManager timerManager;

    [Inject]
    private UpgradeManager upgradeManager;

    [Inject]
    private SoundManager soundManager;

    [Inject]
    private PlayerDialogueController playerDialogue;

    private float currentActiveTime;
    private float currentAbilityCooldown;

    private float startAbilityCooldown;
    private float startActiveTime;

    // Initials
    private UIGameSlower gameSlower;

    private InputAction abilityAction;

    private void Awake()
    {
        gameSlower = GetComponent<UIGameSlower>();
        abilityAction = InputSystem.actions.FindAction("TimeSlow");

        playerDialogue.PlayTextChain(PlayerDialogueChainType.TimeSlowTutorial);
    }

    private void Update()
    {
        if (timerManager.IsPaused)
        {
            return;
        }

        currentActiveTime -= Time.deltaTime;
        activeBar.size = currentActiveTime / startActiveTime;

        if (currentActiveTime < 0)
        {
            currentAbilityCooldown -= Time.deltaTime;

            if (gameSlower.enabled)
            {
                gameSlower.enabled = false;
                abilityCooldownText.gameObject.SetActive(true);
                cooldownBar.gameObject.SetActive(true);
                activeBar.gameObject.SetActive(false);
                button.GetComponent<Image>().color = disabledColor;
            }

            if (currentAbilityCooldown < 0)
            {
                if (abilityAction.WasPressedThisFrame())
                {
                    UseAbility();
                }

                button.interactable = true;
                abilityCooldownText.gameObject.SetActive(false);
                cooldownBar.gameObject.SetActive(false);
                button.GetComponent<Image>().color = Color.white;
            }
            else
            {
                abilityCooldownText.text = Math.Round(currentAbilityCooldown, 2) + "s";
                cooldownBar.size = 1 - currentAbilityCooldown / startAbilityCooldown;
            }
        }
    }

    public void UseAbility()
    {
        if (currentAbilityCooldown > 0 || timerManager.IsPaused)
        {
            return;
        }

        gameSlower.enabled = true;
        button.interactable = false;
        button.GetComponent<Image>().color = disabledColor;

        soundManager.PlayOneShot(soundManager.soundManagerConfig.sfxSlowMotionPath);

        int upgradeAmount = upgradeManager.UpgradeAmount[(int)UpgradeType.Time];

        currentActiveTime = activeTime[upgradeAmount];
        startActiveTime = currentActiveTime;
        activeBar.gameObject.SetActive(true);

        currentAbilityCooldown = abilityCooldown[upgradeAmount];
        startAbilityCooldown = currentAbilityCooldown;
    }
}
