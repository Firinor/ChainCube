using System;
using TMPro;
using UnityEngine;
using Zenject;

public class MaxScoreObserver : MonoBehaviour, IObserver<int>
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
        text.text = "Record : " + value.ToString();
    }

    private void OnEnable()
    {
        player.MaxScore.Subscribe(this);
    }
}
