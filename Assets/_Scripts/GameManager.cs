using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private InitialPlacement[] placement;

    [Inject]
    private CubeFactoryWithPool factory;
    [Inject]
    private Player player; 
    [Inject]
    private GameplayStateMachine stateMachine;

    private IState state;

    private void Awake()
    {
        Cube.OnMerge += WinCheck;
        player.OnLose += LoseGame;
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
        InitialPlacement();
        stateMachine.SetState(State.Game);
    }

    private void WinCheck(int score)
    {
        if (score > 4096)
        {
            stateMachine.SetState(State.Pause);
            Debug.Log("PLAYER WIN!!!");
        }
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
    private void InitialPlacement()
    {
        int randomLevel = Random.Range(0, placement.Length);
        List<CubeWithPosition> cubes = placement[randomLevel].CubeWithPosition;
        foreach(var cube in cubes)
        {
            factory.Create(cube);
        }
    }

    private void OnDestroy()
    {
        Cube.OnMerge -= WinCheck;
        player.OnLose -= LoseGame;
    }
}
