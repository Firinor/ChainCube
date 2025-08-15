public class NormalCubeStatus : PlayerCubeStatus
{
    public NormalCubeStatus(SceneEvents events) : base(events) { }
    
    public override void OnEnter()
    {
        currentCube.CollideEffect = new NormalCube(events);
        base.OnEnter();
    }
}