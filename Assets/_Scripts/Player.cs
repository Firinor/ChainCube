using Zenject;
using UniRx;

public class Player
{
    public IntReactiveProperty MaxScore;
    public IntReactiveProperty CurrentScore;

    [Inject]
    private GameplayStateMachine stateMAchine;
    private State state;

    public void NextCube()
    {
        stateMAchine.SetState(State.CubeAim);
    }
}
