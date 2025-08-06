public class RainbowStatus : PlayerCubeStatus
{
    public RainbowStatus()
    {
        ECube = ECube.Rainbow;
    }
    public override void OnEnter()
    {
        currentCube.Score = (int)ECube.Rainbow;
        currentCube.CollideEffect = new Rainbow();
        base.OnEnter();
    }
}