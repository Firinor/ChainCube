using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using YG;
using Zenject;

public class BestScoreObserver : MonoBehaviour, IObserver<int>
{
    [SerializeField]
    private TMP_Text text;
    [SerializeField]
    private LocalizedString localizedString;

    [Inject]
    private Player player;

    private DateTime time;
    private readonly TimeSpan recordDelay = TimeSpan.FromSeconds(10);

    public void OnCompleted()
    {
    }
    public void OnError(Exception error)
    {
        throw error;
    }
    public void OnNext(int value)
    {
        if(localizedString is null) 
            return;
        
        if (PlayerPrefs.GetInt(PrefsKey.PersonalBestScore) < value)
        {
            localizedString.Arguments[0] = value;
            localizedString.RefreshString();
            player.isNewRecord = true;
            PlayerPrefs.SetInt(PrefsKey.PersonalBestScore, value);

            if (DateTime.Now - time > recordDelay)
            {
                time = DateTime.Now;
                YG2.SetLeaderboard(LeaderboardPanel.BOARDNAME, value);
            }
        }
    }

    [Inject]
    private void Instantiate()
    {
        localizedString.Arguments = new object[] { PlayerPrefs.GetInt(PrefsKey.PersonalBestScore) };
        localizedString.StringChanged += UpdateBestScore;
        localizedString.RefreshString();
        player.CurrentScore.Subscribe(this);
        time = DateTime.Now;
    }

    private void UpdateBestScore(string value)
    {
        text.text = value;
    }
    private void OnDestroy()
    {
        localizedString.StringChanged -= UpdateBestScore; 
    }
}