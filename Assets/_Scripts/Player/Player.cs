using System;
using UniRx;
using UnityEngine;
using Zenject;

public class Player
{
    public IntReactiveProperty CurrentScore = new();
    public bool isNewRecord = false;
    public int oldRecord;
    
    public IntReactiveProperty RainbowCount = new(2);
    public IntReactiveProperty BombCount = new(2);
    public IntReactiveProperty GhostCount = new(2);
    
    [Inject]
    private GameSettings settings;
    [Inject]
    private SceneEvents events;

    private float cooldown;
    private PlayerCubeMachine playerCubeMachine;
    
    [Inject]
    private void Initialize(DiContainer container)
    {
        events.OnMerge += AddScore;
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
                playerCubeMachine.SwitchToStatus(new NormalCubeStatus(events));
                return;
            case ECube.Bomb:
                if(BombCount.Value > 0)
                    playerCubeMachine.SwitchToStatus(new BombStatus(events));
                return;
            case ECube.Rainbow:
                if(RainbowCount.Value > 0)
                    playerCubeMachine.SwitchToStatus(new RainbowStatus(events));
                return;
            case ECube.Ghost:
                if(GhostCount.Value > 0)
                    playerCubeMachine.SwitchToStatus(new GhostStatus(events));
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
        events.OnMerge -= AddScore;
        
        CurrentScore.Dispose();
        BombCount.Dispose();
        RainbowCount.Dispose();
        GhostCount.Dispose();
    }
}
