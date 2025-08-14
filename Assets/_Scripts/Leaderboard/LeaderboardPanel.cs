using System.Linq;
using Firestore;
using UnityEngine;

public class LeaderboardPanel : MonoBehaviour
{
    [SerializeField] 
    private LeaderboardEntryView[] entries;
    [SerializeField] 
    private GameObject BestScores;
    [SerializeField] 
    private GameObject LoadingText;
    
    private void Awake()
    {
        RefreshBoard();
    }

    private void RefreshBoard()
    {
        LoadingText.SetActive(true);
        BestScores.SetActive(false);
        StartCoroutine(new LeaderboardAPI().GetLeaderboardData(OnSuccess));
    }

    private void OnSuccess(LeaderboardData data)
    {
        var sorted = data.documents.OrderByDescending(player => int.Parse(player.Score)).Take(10);;
        int i = 0;
        foreach (var playerRecord in sorted)
        {
            entries[i].Name.text = playerRecord.Name;
            entries[i].Scores.text = playerRecord.Score;
            i++;
        }
        BestScores.SetActive(true);
        LoadingText.SetActive(false);
    }
}
