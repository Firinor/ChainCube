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

    [SerializeField]
    private GameObject LosePanel;
    [SerializeField] 
    private GameObject WinPanel;

    private IState state;

    private void Awake()
    {
        GlobalEvents.OnMerge += BonusCheck;
        GlobalEvents.OnLose += LoseGame;
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
        throw new System.NotImplementedException();
    }

    private void LoseGame()
    {
        stateMachine.SetState(State.Pause);
        if (player.isNewRecord)
            WinPanel.SetActive(true);
        else
            LosePanel.SetActive(true);
    }

    private void CleareScore()
    {
        player.CurrentScore.Value = 0;
    }

    private void OnDestroy()
    {
        GlobalEvents.OnMerge -= BonusCheck;
        GlobalEvents.OnLose -= LoseGame;
    }
}
