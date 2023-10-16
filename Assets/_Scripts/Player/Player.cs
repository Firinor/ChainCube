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
            playerCubeData.Cube = ECube.c2;
            currentPlayerCube = TheFoundCube;
            currentPlayerCube.GetReadyToLaunch();
            cubeFactory.AddToList(TheFoundCube);
        }
        else
        {
            NewCube();
        }
    }
    
    public void SwitchCubeTo(ECube eCube)
    {
        if (currentPlayerCube == null)
            return;

        switch (eCube)
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
                currentPlayerCube.CollideEffect = new Bomb();
                break;
            case ECube.Rainbow:
                currentPlayerCube.CollideEffect = new Rainbow();
                break;
        }
        playerCubeData.Cube = eCube;
        currentPlayerCube.Score = (int)eCube;
        currentPlayerCube.RefreshMaterial();
    }
    public void NewCube()
    {
        playerCubeData.Cube = GetRandomCube();
        currentPlayerCube = cubeFactory.Create(playerCubeData);
        currentPlayerCube.transform.parent = playerCubeAnchor;
        currentPlayerCube.transform.localPosition = Vector3.zero;
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

    private ECube GetRandomCube()
    {
        int random = Random.Range(1, cubeChance.WeightOfAllElements+1);
        int index = 0, weigth = 0;

        while (weigth < random)
        {
            weigth += cubeChance.cubeWeightList[index].Weight;
            index++;
        }

        return cubeChance.cubeWeightList[index-1].Cube;
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
