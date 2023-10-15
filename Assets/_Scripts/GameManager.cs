using System.Collections.Generic;
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
    private InitialPlacement[] placement;

    private IState state;

    private void Awake()
    {
        RestartLevel();
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
}
