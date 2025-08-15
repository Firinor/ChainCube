public class RainbowStatus : PlayerCubeStatus
{
    public RainbowStatus(SceneEvents events) : base(events)
    {
        ECube = ECube.Rainbow;
    }
    public override void OnEnter()
    {
        currentCube.Score = (int)ECube.Rainbow;
        currentCube.CollideEffect = new Rainbow(events);
        base.OnEnter();
    }
    
    public override void Launch(Player player)
    {
        player.RainbowCount.Value--;
        base.Launch(player);
    }
}