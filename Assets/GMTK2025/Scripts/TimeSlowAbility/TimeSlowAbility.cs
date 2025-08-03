using System;
using TMPro;
using UnityEngine;
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

    private float currentActiveTime;
    private float currentAbilityCooldown;

    // Initials
    private UIGameSlower gameSlower;

    private void Awake()
    {
        gameSlower = GetComponent<UIGameSlower>();
    }

    private void Update()
    {
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
        if (currentAbilityCooldown > 0)
        {
            return;
        }

        gameSlower.enabled = true;
        button.interactable = false;

        currentActiveTime = activeTime;
        currentAbilityCooldown = abilityCooldown;
    }
}
