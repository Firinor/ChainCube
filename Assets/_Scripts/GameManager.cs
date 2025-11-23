using System;
using System.Collections;
using FirAnimations;
using UnityEngine;
using YG;
using YG.Utils.LB;
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
    private OneSoundPlayer EndBell;
    [SerializeField]
    private GameObject LosePanel;
    [SerializeField] 
    private NewBestScorePanel WinPanel;
    [SerializeField] 
    private TimerBeforeAdsYG YGTimer;

    private IState state;

    private void Awake()
    {
        events.OnMerge += BonusCheck;
        events.OnLose += MatchEnd;
        //WinPanel.PlayerNameInputField.onEndEdit.AddListener(SavePlayerName);
        stateMachine.SetState(State.Game);

        player.OnPlayerShoot += YGTimer.CheckTimerAd;
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

    [ContextMenu("MatchEnd")]
    public void MatchEnd()
    {
        EndBell.Play();
        Destroy(YGTimer.gameObject);
        StartCoroutine(ToEndScreen());
    }

    private IEnumerator ToEndScreen()
    { 
        yield return new WaitForSeconds(3);
        YG2.onGetLeaderboard += OnSuccessLoad;
        YG2.GetLeaderboard(LeaderboardPanel.BOARDNAME, 10, 1);
    }

    private void OnSuccessLoad(LBData board)
    {
        stateMachine.SetState(State.End);
        YG2.onGetLeaderboard -= OnSuccessLoad;
        if (player.CurrentScore.Value > board.currentPlayer.score)
        {
            WinPanel.gameObject.SetActive(true);
            //WinPanel.TextCounter.EndNumber = player.CurrentScore.Value;
            WinPanel.GetComponent<FirAnimationsManager>().StartAnimations();
        }
        else
        {
            LosePanel.SetActive(true);
            LosePanel.GetComponent<FirAnimationsManager>().StartAnimations();
        }
    }

#if UNITY_EDITOR
    [ContextMenu("ToWinScreen")]
    public void CheatWin()
    {
        stateMachine.SetState(State.Pause);
        WinPanel.gameObject.SetActive(true);
        //WinPanel.TextCounter.EndNumber = player.CurrentScore.Value;
        WinPanel.GetComponent<FirAnimationsManager>().StartAnimations();
    }
    [ContextMenu("ToLoseScreen")]
    public void CheatLose()
    {
        stateMachine.SetState(State.Pause);
        LosePanel.SetActive(true);
        LosePanel.GetComponent<FirAnimationsManager>().StartAnimations();
    }
#endif
    
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
        YG2.onGetLeaderboard -= OnSuccessLoad;
        events.OnMerge -= BonusCheck;
        events.OnLose -= MatchEnd;
        player.OnPlayerShoot -= YGTimer.CheckTimerAd;
        //WinPanel.PlayerNameInputField.onEndEdit.RemoveListener(SavePlayerName);
    }
}