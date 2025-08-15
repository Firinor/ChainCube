public class GhostStatus : PlayerCubeStatus
{
    public GhostStatus(SceneEvents events) : base(events)
    {
        ECube = ECube.Ghost;
    }
    public override void OnEnter()
    {
        currentCube.Collider.enabled = false;
        Cubid сachedPlayerCubid = new Cubid()
        {
            Score = (ECube)currentCube.Score,
            Form = currentCube.form
        };
        currentCube.CollideEffect = new Ghost(сachedPlayerCubid, cubeFactory, events);
        currentCube.Score = (int)ECube.Ghost;
        base.OnEnter();
    }

    public override void OnExit()
    {
        currentCube.Collider.enabled = true;
    }

    public override void Launch(Player player)
    {
        player.GhostCount.Value--;
        base.Launch(player);
    }
}