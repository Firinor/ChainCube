public class NormalCubeStatus : PlayerCubeStatus
{
    public override void OnEnter()
    {
        currentCube.CollideEffect = new NormalCube();
        base.OnEnter();
    }
}