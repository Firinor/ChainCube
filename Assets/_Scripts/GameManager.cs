using System;
using System.Collections;
using FirAnimations;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject]
    private CubeFactoryWithPool factory;
    [Inject]
    private Player player; 
    [Inject]
    private GameplayStateMachine stateMachine;
    [Inject] 
    private SceneEvents events;

    [SerializeField]
    private GameObject LosePanel;
    [SerializeField] 
    private NewBestScorePanel WinPanel;

    private IState state;

    private void Awake()
    {
        events.OnMerge += BonusCheck;
        events.OnLose += MatchEnd;
        WinPanel.PlayerNameInputField.onEndEdit.AddListener(SavePlayerName);
        stateMachine.SetState(State.Game);
    }
    private void Update()
    {
        stateMachine.Tick();
    }

    public void RestartLevel()
    {
        CleareScore();
        factory.ClearAll();
        stateMachine.SetState(State.Game);
    }

    private void BonusCheck(Cube c1, Cube c2)
    {
        if (c1.Score == 4096 && c2.Score == 4096)
        {
            AddRandomBonus();
        }
    }

    private void AddRandomBonus()
    {
        //TODO
    }

    private void MatchEnd()
    {
        stateMachine.SetState(State.Pause);
        StartCoroutine(ToEndScreen());
        
    }

    private IEnumerator ToEndScreen()
    {
        yield return new WaitForSeconds(3);
        if (player.isNewRecord)
        {
            WinPanel.gameObject.SetActive(true);
            WinPanel.GetComponent<FirAnimationsManager>().StartAnimations();
        }
        else
        {
            LosePanel.SetActive(true);
            LosePanel.GetComponent<FirAnimationsManager>().StartAnimations();
        }
    }

    [ContextMenu("ToWinScreen")]
    private void CheatWin()
    {
        stateMachine.SetState(State.Pause);
        WinPanel.gameObject.SetActive(true);
        WinPanel.TextCounter.EndNumber = player.CurrentScore.Value;
        WinPanel.GetComponent<FirAnimationsManager>().StartAnimations();
    }
    [ContextMenu("ToLoseScreen")]
    private void CheatLose()
    {
        stateMachine.SetState(State.Pause);
        LosePanel.SetActive(true);
        LosePanel.GetComponent<FirAnimationsManager>().StartAnimations();
    }
    private void CleareScore()
    {
        player.CurrentScore.Value = 0;
    }

    public void SavePlayerName(string newName)
    {
        if(!String.IsNullOrEmpty(newName))
            PlayerPrefs.SetString(PrefsKey.PlayerName, newName);
    }
    
    private void OnDestroy()
    {
        events.OnMerge -= BonusCheck;
        events.OnLose -= MatchEnd;
        WinPanel.PlayerNameInputField.onEndEdit.RemoveListener(SavePlayerName);
    }
}
