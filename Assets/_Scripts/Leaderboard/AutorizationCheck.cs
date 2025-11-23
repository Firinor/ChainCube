using UnityEngine;
using YG;
using YG.Utils.LB;
using Zenject;

public class AutorizationCheck : MonoBehaviour
{
    public GameObject Leaderboard;
    public GameObject AutorizationPanel;

    [SerializeField] private LeaderboardPanel panel;
    [SerializeField] private BestScoreObserver bestScore;
    [Inject] private Player player;

    private void Awake()
    {
        YG2.onGetSDKData += CheckRecord;
    }

    private void CheckRecord()
    {
        YG2.onGetSDKData -= CheckRecord;

        YG2.onGetLeaderboard += OnGetLeaderboard;
        YG2.GetLeaderboard(LeaderboardPanel.BOARDNAME, 1, 1);
    }

    private void OnGetLeaderboard(LBData data)
    {
        YG2.onGetLeaderboard -= OnGetLeaderboard;

        int record = Mathf.Max(
            player.CurrentScore.Value,
            PlayerPrefs.GetInt(PrefsKey.PersonalBestScore, defaultValue: 0));

        if (YG2.player.auth)
        {
            record = Mathf.Max(
                record,
                data.currentPlayer.score);
        }

        PlayerPrefs.SetInt(PrefsKey.PersonalBestScore, record);
        
        YG2.SetLeaderboard(LeaderboardPanel.BOARDNAME, record);
        
        bestScore.SetBestScore();
    }

    public void TryOpenLeaderboard()
    {
        if (YG2.player.auth)
        {
            Leaderboard.SetActive(true);
            panel.RefreshBoard();
        }
        else
            AutorizationPanel.SetActive(true);
    }
    
    public void Autorize()
    {
        YG2.OpenAuthDialog();
        AutorizationPanel.SetActive(false);
    }
}
