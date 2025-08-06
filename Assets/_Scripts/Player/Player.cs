using System;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Player
{
    public IntReactiveProperty CurrentScore = new();
    
    [Inject]
    private GameSettings settings;

    private float cooldown;
    private PlayerCubeMachine playerCubeMachine;
    
    [Inject]
    private void Initialize(DiContainer container)
    {
        GlobalEvents.OnMerge += AddScore;
        playerCubeMachine = container.Resolve<PlayerCubeMachine>();
        Cube playerStartCube = container.ResolveId<Transform>("PlayerCubeAnchor").GetComponentInChildren<Cube>();
        playerCubeMachine.NewCube(playerStartCube);
    }

    public void SwitchCubeTo(int cube)
    {
        switch ((ECube)cube)
        {
            case ECube.c2:
            case ECube.c4:
            case ECube.c8:
            case ECube.c16:
            case ECube.c32:
            case ECube.c64:
            case ECube.c128:
            case ECube.c256:
            case ECube.c512:
            case ECube.c1024:
            case ECube.c2048:
            case ECube.c4096:
                playerCubeMachine.SwitchToStatus(new NormalCubeStatus());
                return;
            case ECube.Bomb:
                playerCubeMachine.SwitchToStatus(new BombStatus());
                return;
            case ECube.Rainbow:
                playerCubeMachine.SwitchToStatus(new RainbowStatus());
                return;
            case ECube.Ghost:
                playerCubeMachine.SwitchToStatus(new GhostStatus());
                return;
        }

        throw new Exception();
    }
    
    public void NewCube()
    {
        playerCubeMachine.NewCube();
    }
    public void Cooldown(float deltaTime)
    {
        if(cooldown > 0)
        {
            cooldown -= deltaTime;
            //At cooldown end
            if(cooldown <= 0)
            {
                NewCube();
            }
        }
    }
    public void TryShoot()
    {
        if(cooldown <= 0)
        {
            playerCubeMachine.TryShoot();
            cooldown += settings.CubeReloadTime;
        }
    }
    
    private void AddScore(Cube c1, Cube c2)
    {
        CurrentScore.Value += c1.Score;
    }
    
    ~Player()
    {
        GlobalEvents.OnMerge -= AddScore;
    }
}
