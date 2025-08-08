using UnityEngine;

public class BombStatus : PlayerCubeStatus
{
    private Cube bomb;

    public BombStatus()
    {
        ECube = ECube.Bomb;
    }
    public override void OnEnter()
    {
        currentCube.RemoveCube();
        bomb = cubeFactory.GetBomb();
        bomb.transform.localPosition = Vector3.zero;
        bomb.GetReadyToLaunch();
    }

    public override void OnExit()
    {
        bomb.RemoveCube();
        cubeFactory.ToPlayerHand(currentCube);
        currentCube.GetReadyToLaunch();
    }
    public override void Launch(Player player)
    {
        player.BombCount.Value--;
        bomb.transform.parent = cubeFactory.transform;
        bomb.Launch();
    }
}