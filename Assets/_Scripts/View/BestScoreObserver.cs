using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

public class BestScoreObserver : MonoBehaviour, IObserver<int>
{
    [SerializeField]
    private TMP_Text text;
    [SerializeField]
    private LocalizedString localizedString;

    [Inject]
    private Player player;

    public void OnCompleted()
    {
    }
    public void OnError(Exception error)
    {
        throw error;
    }
    public void OnNext(int value)
    {
        if(localizedString is null) return;
        
        if (PlayerPrefs.GetInt(PrefsKey.PersonalBestScore) < value)
        {
            localizedString.Arguments[0] = value;
            localizedString.RefreshString();
            player.isNewRecord = true;
            PlayerPrefs.SetInt(PrefsKey.PersonalBestScore, value);
        }
    }

    [Inject]
    private void Instantiate()
    {
        localizedString.Arguments = new object[] { PlayerPrefs.GetInt(PrefsKey.PersonalBestScore) };
        localizedString.StringChanged += UpdateBestScore;
        localizedString.RefreshString();
        player.CurrentScore.Subscribe(this);
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