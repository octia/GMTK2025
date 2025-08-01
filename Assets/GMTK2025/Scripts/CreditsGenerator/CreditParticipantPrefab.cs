using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class CreditParticipantPrefab : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;

    [SerializeField]
    private Button button;

    [SerializeField]
    private string link;

    internal void Initialize(Participant participantData)
    {
        text.text = participantData.Name.Trim();
        link = participantData.Link.Trim();
    }

    private void Start()
    {
        if (link != "")
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnButtonClicked(link));
        }
        else
        {
            button.interactable = false;
        }
    }

    private void OnButtonClicked(string link)
    {
        Application.OpenURL(link);
    }
}
