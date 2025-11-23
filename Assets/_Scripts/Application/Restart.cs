using UnityEngine;
using UnityEngine.SceneManagement;
using YG;
using YG.Utils.LB;
using Zenject;

public class Restart : MonoBehaviour
{
    [Inject]
    private Player player;

    private int currentScore;

    public void OnClickRestart()
    {
        currentScore = player.CurrentScore.Value;
        if (currentScore > PlayerPrefs.GetInt(PrefsKey.PersonalBestScore))
            PlayerPrefs.SetInt(PrefsKey.PersonalBestScore, currentScore);
        YG2.onGetLeaderboard += OnSuccessLoad;
        YG2.GetLeaderboard(LeaderboardPanel.BOARDNAME, 1, 1);
    }

    private void OnSuccessLoad(LBData board)
    {
        YG2.onGetLeaderboard -= OnSuccessLoad;
        if (currentScore >= PlayerPrefs.GetInt(PrefsKey.PersonalBestScore)
            && currentScore > board.currentPlayer.score)
            YG2.SetLeaderboard(LeaderboardPanel.BOARDNAME, currentScore);
        SceneManager.LoadScene(0);
    }
}
