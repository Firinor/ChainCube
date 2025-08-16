using System.Linq;
using Firestore;
using UnityEngine;
using Zenject;

public class LeaderboardPanel : MonoBehaviour
{
    [SerializeField] 
    private LeaderboardEntryView[] entries;
    [SerializeField] 
    private GameObject BestScores;
    [SerializeField] 
    private GameObject LoadingText;
    [Inject]
    private Player player;
    
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

        player.oldRecord = int.Parse(entries[i-1].Scores.text);
        BestScores.SetActive(true);
        LoadingText.SetActive(false);
    }
}
