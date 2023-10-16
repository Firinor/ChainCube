using UnityEngine;
using Zenject;

public class InGameRules : IState
{
    [Inject]
    private Player player;
    [Inject]
    private PlayerInput playerInput;

    public void Enter()
    {
        playerInput.gameObject.SetActive(true);
    }
    public void Tick()
    {
        player.Cooldown(Time.deltaTime);
    }
    public void Exit()
    {
        playerInput.gameObject.SetActive(false);
    }

}