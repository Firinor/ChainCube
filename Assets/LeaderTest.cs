using Firestore;
using UnityEngine;

public class LeaderTest : MonoBehaviour
{
    public string Name;
    public int Score;

    [ContextMenu("Get")]
    public void Get()
    {
        var leaderboard = new LeaderboardAPI();
        StartCoroutine(leaderboard.Start());
    }
    [ContextMenu("Send")]
    public async void Send()
    {
        var leaderboard = new LeaderboardAPI();
        await leaderboard.CreateDocumentAsync(Name, Score, this);
    }
}
