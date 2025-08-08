public abstract class PlayerCubeStatus
{
    public CubeFactoryWithPool cubeFactory;
    public Cube currentCube;
    public ECube ECube { get; protected set; }

    public virtual void OnEnter()
    {
        cubeFactory.RefreshView(currentCube);
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