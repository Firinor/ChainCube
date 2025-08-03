using System;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Player
{
    public IntReactiveProperty CurrentScore = new();

    public Action OnLose;

    [Inject]
    private CubeFactoryWithPool cubeFactory;
    [Inject]
    private CubeChanceWeight cubeChance;
    [Inject(Id = "PlayerCubeAnchor")]
    private Transform playerCubeAnchor;
    [Inject]
    private GameSettings settings;
    [Inject]
    private LoseBox loseBox;

    private Cube currentPlayerCube;
    private CubeWithPosition playerCubeData = new();
    private float cooldown;

    [Inject]
    private void Initialize()
    {
        Cube.OnMerge += AddScore;
    }

    public void InitializeFirstCube()
    {
        Cube TheFoundCube = playerCubeAnchor.GetComponentInChildren<Cube>();
        if(TheFoundCube != null)
        {
            playerCubeData.Cubid = new()
            {
                Score = ECube.c2,
                Form = ECubeForm.Cube
            };
            currentPlayerCube = TheFoundCube;
            currentPlayerCube.GetReadyToLaunch();
            cubeFactory.AddToList(TheFoundCube);
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
                //currentPlayerCube = cubeFactory.GetBomb;
                break;
            case ECube.Rainbow:
                currentPlayerCube.CollideEffect = new Rainbow();
                break;
            case ECube.Ghost:
                currentPlayerCube.CollideEffect = new Ghost(currentPlayerCube.Collider);
                break;
        }
        playerCubeData.Cubid = cubid;
        currentPlayerCube.Score = (int)cubid.Score;
        currentPlayerCube.RefreshMaterial();
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
                if (loseBox.IsLose)
                {
                    OnLose?.Invoke();//GAME OVER
                    return;
                }
                else
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
    private void AddScore(int points)
    {
        CurrentScore.Value += points;
    }
    
    ~Player()
    {
        Cube.OnMerge -= AddScore;
    }
}
