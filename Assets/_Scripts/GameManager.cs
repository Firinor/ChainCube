using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject]
    private CubeFactoryWithPool factory;
    [Inject]
    private Player player;
    [SerializeField]
    private InitialPlacement placement;

    public void RestartLevel()
    {
        CleareScore();
        factory.ClearAll();
        InitialPlacement();
        player.NextCube();
    }

    private void CleareScore()
    {
        player.CurrentScore.Value = 0;
    }

    private void InitialPlacement()
    {
        var cubes = placement.CubeWithPosition;
        foreach(var cube in cubes)
        {
            factory.Create(cube.Position);
        }
    }
}
