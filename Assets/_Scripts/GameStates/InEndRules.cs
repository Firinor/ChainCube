using Zenject;

public class InEndRules : IState
{
    [Inject]
    private Player player;
    [Inject]
    private CubeFactoryWithPool pool;
    
    public void Enter()
    {
        pool.FreezeAll();
    }

    public void Exit()
    {
        
    }

    public void Tick()
    {
        
    }
}