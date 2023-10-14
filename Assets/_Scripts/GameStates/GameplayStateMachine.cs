using System.Collections.Generic;
using Zenject;

public class GameplayStateMachine
{
    private State currentState;
    private IState currentIState;
    private Dictionary<State, IState> states;

    public void SetState(State newState) {

        if(newState != currentState)
        {
            IState state = states[newState];

            currentIState.Exit();
            currentIState = state;
            currentIState.Enter();
        }
    }

    [Inject]
    private void Initialize()
    {
        states = new Dictionary<State, IState>();
        states.Add(State.Pause, new Pause());
    //    states.Add(State.CubeAim, new CubeAim());
    //    states.Add(State.Shoot, new Shoot());
    }
}
