using System.Collections.Generic;
using System.Threading.Tasks;
using EditorAttributes;
using Reflex.Attributes;
using UnityEngine;

public class ScorePanel : MonoBehaviour
{
    [Inject] private LeaderboardServerBridge leaderboardServerBridge;

    [Inject] private FinanceManager financeManager;


    public int Score { get; private set; }

    private void Awake()
    {
        financeManager.OnProfitGenerated.AddListener(AddToScore);
    }

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
            name = "Anonymous Controller";
            UpdateName(name);
        }
        await leaderboardServerBridge.SendUserValue(name, Score);
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
