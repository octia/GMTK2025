using System.Linq;
using PrimeTween;
using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerDialogueController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject dialogueBox;

    [SerializeField]
    private TMP_Text dialogueTextBox;

    [SerializeField]
    private Image speakerIcon;

    [SerializeField]
    [Multiline]
    private PlayerDialogueChain debugDialogue;

    [SerializeField]
    private InputAction skipAction;

    [Inject]
    private PlayerDialogueConfig config;

    private Tween textAnimTween;

    private PlayerDialogueChain currentChain;

    private int currentDialogueIndex = 0;

    public void PlayText(string message, float lettersPerSecond, Sprite overrideSprite = null)
    {
        SetEnabled(true);

        speakerIcon.sprite = overrideSprite != null ? overrideSprite : config.DefaultIcon;
        dialogueTextBox.text = message;

        textAnimTween = Tween.Custom(
            0,
            message.Length,
            message.Length / lettersPerSecond,
            (val) => dialogueTextBox.maxVisibleCharacters = Mathf.RoundToInt(val),
            Ease.Linear
        );
    }

    public void PlayTextChain(PlayerDialogueChainType type)
    {
        if (type == PlayerDialogueChainType.Empty)
        {
            return;
        }

        var chainToUse = config.PlayerDialogueChains.FirstOrDefault(
            (chain) => chain.ChainType == type
        );
        if (chainToUse == null)
        {
            return;
        }

        SetEnabled(true);
        currentChain = chainToUse;
        currentDialogueIndex = 0;
        PlayCurrentDialogue();
    }

    private void PlayCurrentDialogue()
    {
        if (currentChain == null)
        {
            return;
        }

        if (currentDialogueIndex >= currentChain.Dialogues.Count)
        {
            SetEnabled(false);
            currentChain = null;
            return;
        }

        var currentDialogue = currentChain.Dialogues[currentDialogueIndex];
        PlayText(currentDialogue.Message, config.CharactersPerSecond, currentDialogue.IconOverride);
        currentDialogueIndex++;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ProgressDialogue();
    }

    public void ProgressDialogue()
    {
        if (textAnimTween.isAlive)
        {
            textAnimTween.Complete();
            return;
        }

        PlayCurrentDialogue();
    }

    private void Awake()
    {
        PlayTextChain(PlayerDialogueChainType.InitialTutorial);
    }

    private void SetEnabled(bool status)
    {
        if (dialogueBox.activeSelf == status)
        {
            return;
        }

        if (status)
        {
            dialogueBox.SetActive(status);
        }

        var startTweenValue = status ? 0f : 1f;
        var endTweenValue = status ? 1f : 0f;
        var longerTween = Tween.ScaleX(dialogueBox.transform, startTweenValue, endTweenValue, 0.7f);
        Tween.ScaleY(dialogueBox.transform, startTweenValue, endTweenValue, 0.4f);

        if (!status)
        {
            longerTween.OnComplete(() => dialogueBox.SetActive(false));
        }
    }
}
