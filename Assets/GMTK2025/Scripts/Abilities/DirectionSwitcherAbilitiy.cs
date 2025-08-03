using System;
using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DirectionSwitcherAbilitiy : MonoBehaviour
{
    [SerializeField]
    private float[] abilityCooldown;

    [Space]
    [SerializeField]
    private Button button;

    [SerializeField]
    private Scrollbar scrollbar;

    [SerializeField]
    private TMP_Text abilityCooldownText;

    [SerializeField]
    private Color disabledColor;

    [Inject]
    private CustomCursor customCursor;

    [Inject]
    private TimerManager timerManager;

    [Inject]
    private UpgradeManager upgradeManager;

    private float currentAbilityCooldown;

    private float startAbilityCooldown;

    private bool targetMode;

    // Initials
    private InputAction abilityAction;
    private InputAction interactAction;

    private void Awake()
    {
        abilityAction = InputSystem.actions.FindAction("DirectionSwitcher");
        interactAction = InputSystem.actions.FindAction("Click");
    }

    private void Update()
    {
        if (timerManager.IsPaused)
        {
            return;
        }

        currentAbilityCooldown -= Time.deltaTime;

        if (timerManager.IsPaused && targetMode)
        {
            SetTargetMode(false);
        }

        if (currentAbilityCooldown < 0)
        {
            if (abilityAction.WasPressedThisFrame())
            {
                UseAbility();
            }

            button.interactable = true;
            abilityCooldownText.gameObject.SetActive(false);
            scrollbar.gameObject.SetActive(false);
            button.GetComponent<Image>().color = Color.white;
        }
        else
        {
            abilityCooldownText.text = Math.Round(currentAbilityCooldown, 2) + "s";
            scrollbar.size = 1 - currentAbilityCooldown / startAbilityCooldown;
        }
    }

    private void LateUpdate()
    {
        if (targetMode && interactAction.WasPressedThisFrame())
        {
            SetTargetMode(false);
            abilityCooldownText.gameObject.SetActive(true);
            scrollbar.gameObject.SetActive(true);
            button.GetComponent<Image>().color = disabledColor;

            int upgradeAmount = upgradeManager.UpgradeAmount[(int)UpgradeType.Direction];
            currentAbilityCooldown = abilityCooldown[upgradeAmount];
        }  
    }

    public void UseAbility()
    {
        if (currentAbilityCooldown > 0 || timerManager.IsPaused)
        {
            return;
        }

        SetTargetMode(true);

        button.interactable = false;
        button.GetComponent<Image>().color = disabledColor;

        int upgradeAmount = upgradeManager.UpgradeAmount[(int)UpgradeType.Direction];
        currentAbilityCooldown = abilityCooldown[upgradeAmount];
        startAbilityCooldown = currentAbilityCooldown;
    }

    private void SetTargetMode(bool value)
    {
        customCursor.TargetMode = value;
        targetMode = value;

        customCursor.ChangeCursorSprite(value ? 2 : 0);
    }
}