using System;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Player
{
    public IntReactiveProperty MaxScore = new();
    public IntReactiveProperty CurrentScore = new();

    [Inject]
    private CubeFactoryWithPool cubeFactory;
    [Inject]
    private CubeChanceWeight cubeChance;

    private CubeWithPosition playerCube;

    [SerializeField]
    private float board;

    public void NextCube()
    {
        InitializeNewCube();
    }

    private void InitializeNewCube()
    {
        playerCube.Cube = GetRandomCube();
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

    public void SetCubePosition(Vector3 position)
    {
        playerCube.Position = position;
    }

    public void Shoot()
    {
        cubeFactory.Create(playerCube);
    }
}
