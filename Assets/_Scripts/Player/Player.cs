using System;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Player
{
    public IntReactiveProperty CurrentScore = new();

    [Inject]
    private CubeFactoryWithPool cubeFactory;
    [Inject]
    private CubeChanceWeight cubeChance;
    [Inject(Id = "PlayerCubeAnchor")]
    private Transform playerCubeAnchor;
    [Inject]
    private GameSettings settings;

    private Cube currentPlayerCube;
    private CubeWithPosition playerCubeData = new();
    private float cooldown;

    [Inject]
    private void Initialize()
    {
        GlobalEvents.OnMerge += AddScore;
    }

    public void InitializeFirstCube()
    {
        Cube TheFoundCube = playerCubeAnchor.GetComponentInChildren<Cube>();
        if(TheFoundCube != null)
        {
            playerCubeData.Cubid = new()
            {
                Score = (ECube)TheFoundCube.Score,
                Form = TheFoundCube.form
            };
            currentPlayerCube = TheFoundCube;
            currentPlayerCube.GetReadyToLaunch();
            currentPlayerCube.CollideEffect = new NormalCube();
        }
        else
        {
            NewCube();
        }
    }

    public void SwitchCubeTo(int cube)
    {
        SwitchCubeTo(cubid: new(){Score = (ECube)cube, Form = playerCubeData.Cubid.Form});
    }

    public void SwitchCubeTo(Cubid cubid)
    {
        if (currentPlayerCube == null)
            return;

        switch (cubid.Score)
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
                currentPlayerCube.CollideEffect = new NormalCube();
                break;
            case ECube.Bomb:
                if (currentPlayerCube == cubeFactory.Bomb)
                {
                    Bomb bomb = currentPlayerCube.CollideEffect as Bomb;
                    currentPlayerCube.RemoveCube();
                    currentPlayerCube = bomb.Cancel();
                }
                else
                {
                    currentPlayerCube.RemoveCube();
                    currentPlayerCube = cubeFactory.CreateBomb(currentPlayerCube);
                    currentPlayerCube.transform.SetParent(playerCubeAnchor);
                    currentPlayerCube.transform.localPosition = Vector3.zero;
                }
                currentPlayerCube.GetReadyToLaunch();
                return;
            case ECube.Rainbow:
                currentPlayerCube.CollideEffect = new Rainbow();
                break;
            case ECube.Ghost:
                currentPlayerCube.CollideEffect = new Ghost(currentPlayerCube.Collider);
                break;
        }
        playerCubeData.Cubid = cubid;
        currentPlayerCube.Score = (int)cubid.Score;
        cubeFactory.RefreshView(currentPlayerCube);
    }
    public void NewCube()
    {
        playerCubeData.Cubid = GetRandomCubid();
        currentPlayerCube = cubeFactory.Create(playerCubeData);
        currentPlayerCube.transform.parent = playerCubeAnchor;
        currentPlayerCube.GetReadyToLaunch();
    }

    public void SetCubePosition(Vector3 position)
    {
        playerCubeData.Position = position;
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
            currentPlayerCube.transform.parent = cubeFactory.transform;
            currentPlayerCube.Launch();
            currentPlayerCube = null;
            cooldown += settings.CubeReloadTime;
        }
    }

    private Cubid GetRandomCubid()
    {
        int random = Random.Range(1, cubeChance.WeightOfAllElements+1);
        int index = 0, weigth = 0;

        while (weigth < random)
        {
            weigth += cubeChance.cubeWeightList[index].Weight;
            index++;
        }

        int randomForm = Random.Range(0, maxExclusive: 2);

        Cubid result = new()
        {
            Score = cubeChance.cubeWeightList[index-1].Cube,
            Form = (ECubeForm)randomForm
        };
        
        return result;
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
