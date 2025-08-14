using System;
using TMPro;
using UnityEngine;
using Zenject;

public class BestScoreObserver : MonoBehaviour, IObserver<int>
{
    [SerializeField]
    private TMP_Text text;

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
        if (PlayerPrefs.GetInt(PrefsKey.PersonalBestScore) < value)
        {
            text.text = "Best Score : " + value.ToString();
            player.isNewRecord = true;
            PlayerPrefs.SetInt(PrefsKey.PersonalBestScore, value);
        }
    }

    [Inject]
    private void Instantiate()
    {
        player.CurrentScore.Subscribe(this);
        text.text = "Best Score : " + PlayerPrefs.GetInt(PrefsKey.PersonalBestScore);
    }
}