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
        states.Add(State.Pause, new InPauseRules());
        states.Add(State.Game, new InGameRules());

        currentIState = states[State.Game];
    }

    public void Tick()
    {
        currentIState.Tick();
    }
}
