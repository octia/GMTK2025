using System;
using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TimeSlowAbility : MonoBehaviour
{
    [SerializeField]
    private float activeTime;

    [SerializeField]
    private float abilityCooldown;

    [Space]
    [SerializeField]
    private Button button;

    [SerializeField]
    private TMP_Text abilityCooldownText;

    [Inject]
    private TimerManager timerManager;

    private float currentActiveTime;
    private float currentAbilityCooldown;

    // Initials
    private UIGameSlower gameSlower;

    private InputAction abilityAction;

    private void Awake()
    {
        gameSlower = GetComponent<UIGameSlower>();
        abilityAction = InputSystem.actions.FindAction("TimeSlow");
    }

    private void Update()
    {
        if (timerManager.IsPaused)
        {
            return;
        }
        
        currentActiveTime -= Time.deltaTime;

        if (currentActiveTime < 0)
        {
            currentAbilityCooldown -= Time.deltaTime;

            if (gameSlower.enabled)
            {
                gameSlower.enabled = false;
                abilityCooldownText.gameObject.SetActive(true);
            }

            if (currentAbilityCooldown < 0)
            {
                if (abilityAction.WasPressedThisFrame())
                {
                    UseAbility();
                }

                button.interactable = true;
                abilityCooldownText.gameObject.SetActive(false);
            }
            else
            {
                abilityCooldownText.text = Math.Round(currentAbilityCooldown, 2) + "s";
            }
        }
    }

    public void UseAbility()
    {
        if (currentAbilityCooldown > 0 || !timerManager.IsPaused)
        {
            return;
        }

        gameSlower.enabled = true;
        button.interactable = false;

        currentActiveTime = activeTime;
        currentAbilityCooldown = abilityCooldown;
    }
}
