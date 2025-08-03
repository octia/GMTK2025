using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text scoreText;

    public void SetScoreEntry(int rank, string name, string score)
    {
        rankText.text = $"{rank}.";
        nameText.text = name;
        scoreText.text = score;
    }
}
