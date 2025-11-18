using UnityEngine;
using UnityEngine.SceneManagement;
using YG;
using YG.Utils.LB;
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
    
    public const string BOARDNAME = "CubidsLeaderboard";

    private void Awake()
    {
        RefreshBoard();
    }

    private void RefreshBoard()
    {
        LoadingText.SetActive(true);
        BestScores.SetActive(false);
        YG2.onGetLeaderboard += OnSuccessLoad;
        YG2.GetLeaderboard(BOARDNAME, 10, 1);
    }

    private void OnSuccessLoad(LBData data)
    {
        //var sorted = .OrderByDescending(player => player.score).Take(10);
        int i = 0;
        foreach (LBPlayerData playerRecord in data.players)
        {
            entries[i].Name.text = playerRecord.name;
            entries[i].Scores.text = playerRecord.score.ToString();
            i++;
        }

        player.oldRecord = int.Parse(entries[i-1].Scores.text);
        BestScores.SetActive(true);
        LoadingText.SetActive(false);
    }

    public void SetNewRecord()
    {
        YG2.SetLeaderboard(BOARDNAME, player.CurrentScore.Value);
        SceneManager.LoadScene(0);
    }

    private void OnDestroy()
    {
        YG2.onGetLeaderboard -= OnSuccessLoad;
    }
}
