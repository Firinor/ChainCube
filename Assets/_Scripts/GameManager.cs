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
    private LoseBox loseBox;

    private IState state;

    private void Awake()
    {
        Cube.OnMerge += BonusCheck;
        loseBox.OnLose += LoseGame;
        RestartLevel();
        player.InitializeFirstCube();
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

    private void BonusCheck(int score)
    {
        if (score > 4096)
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
        Debug.Log("GAME OVER!!!");
    }
    private void CleareScore()
    {
        player.CurrentScore.Value = 0;
    }

    private void OnDestroy()
    {
        Cube.OnMerge -= BonusCheck;
        loseBox.OnLose -= LoseGame;
    }
}
