using System.Collections.Generic;
using System.Threading.Tasks;
using EditorAttributes;
using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    [Inject] private LeaderboardServerBridge leaderboardServerBridge;

    [Inject] private FinanceManager financeManager;

    [SerializeField] private ScoreDisplay[] scoreDisplays;
    [SerializeField] private TMP_InputField nameInputField;

    public int Score { get; private set; }

    private void AddToScore(int profit) => Score += profit;

    [Button]
    public void UpdateName(string name)
    {
        PlayerPrefs.SetString("Player Name", name);
    }

    public string GetName()
    {
        return PlayerPrefs.GetString("Player Name", "");
    }

    [Button]
    public async Task UploadScore()
    {
        string name = GetName();
        if (name.IsNullOrEmpty())
        {
            name = "Employee 80";
            UpdateName(name);
        }
        await leaderboardServerBridge.SendUserValue(name, Score);
    }

    [Inject]
    public void Init()
    {
        financeManager.OnProfitGenerated.AddListener(AddToScore);
        nameInputField.text = GetName();
        nameInputField.onValueChanged.AddListener(UpdateName);
    }

    private async void OnEnable()
    {
        foreach(var display in scoreDisplays)
        {
            display.SetScoreEntry(0, "", "");
        }
        await UploadScore();
        var (highEntries, ownEntry) = await GetScores();
        if (highEntries != null)
        {
            for (int i = 0; i < highEntries.Count; i++)
            {
                var entry = highEntries[i];
                scoreDisplays[i].SetScoreEntry(entry.position, entry.name, $"${entry.GetValueAsString()}");
            }
        }
        if (ownEntry != null)
        {
            var entry = ownEntry;
            scoreDisplays[3].SetScoreEntry(entry.position, entry.name, $"${entry.GetValueAsString()} (${Score})");
        }
        else
        {
            scoreDisplays[3].SetScoreEntry(1, GetName(), Score.ToString());
        }
    }

    public async Task<(List<LeaderboardEntry> highEntries, LeaderboardEntry ownEntry)> GetScores()
    {
        List<LeaderboardEntry> highEntries = await leaderboardServerBridge.RequestEntries(1, 3);
        LeaderboardEntry ownEntry = await leaderboardServerBridge.RequestUserEntry(GetName());
        return (highEntries, ownEntry);
    }

#pragma warning disable IDE0051 // Remove unused private members
    [Button]
    private async void DebugScores()

    {
        var scores = await GetScores();
        if(scores.highEntries != null)
        {
            foreach (var entry in scores.highEntries)
            {
                Debug.Log($"{entry.position} - {entry.GetValueAsInt()} - {entry.name}");
            }
        }
        if(scores.ownEntry != null)
        {
            Debug.Log($"{scores.ownEntry.position} - {scores.ownEntry.GetValueAsInt()} - {scores.ownEntry.name}");
        }
    }
#pragma warning restore IDE0051 // Remove unused private members
}
