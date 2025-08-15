public abstract class PlayerCubeStatus
{
    public CubeFactoryWithPool cubeFactory;
    public Cube currentCube;
    protected SceneEvents events;
    public ECube ECube { get; protected set; }

    public PlayerCubeStatus(SceneEvents events)
    {
        this.events = events;
    }
    
    public virtual void OnEnter()
    {
        cubeFactory.RefreshView(currentCube);
        currentCube.CollideEffect.events = events;
        currentCube.GetReadyToLaunch();
    }

    public virtual void OnExit()
    {
        currentCube.RemoveCube();
    }

    public virtual void Launch(Player player)
    {
        currentCube.transform.parent = cubeFactory.transform;
        currentCube.Launch();
    }
}