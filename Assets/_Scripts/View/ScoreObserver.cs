using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

public class ScoreObserver : MonoBehaviour, IObserver<int>
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
        
        localizedString.Arguments[0] = value;
        localizedString.RefreshString();
    }

    [Inject]
    private void Instantiate()
    {
        localizedString.Arguments = new object[] { player.CurrentScore.Value };
        localizedString.StringChanged += UpdateScore; 
        player.CurrentScore.Subscribe(this);
    }
    private void UpdateScore(string value)
    {
        text.text = value;
    }

    private void OnDestroy()
    {
        localizedString.StringChanged -= UpdateScore; 
    }
}