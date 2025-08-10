using UnityEngine;

public class LeaderTest : MonoBehaviour
{
    public string Name;
    public int Score;
    
    private DreamloLeaderboard leaderboard = new();

    [ContextMenu("SendScore")]
    public void SendScore()
    {
        leaderboard = new();

        StartCoroutine(leaderboard.AddScore(Name, Score));
    }
    [ContextMenu("GetScore")]
    public void GetScore()
    {
        leaderboard = new();

        StartCoroutine(leaderboard.DownloadTopScores());
    }
}
