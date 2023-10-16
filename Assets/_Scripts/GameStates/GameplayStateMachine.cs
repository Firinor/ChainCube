using System.Collections.Generic;
using Zenject;

public class GameplayStateMachine
{
    private State currentState;
    private IState currentIState;
    private Dictionary<State, IState> states;

    [Inject]
    private void Initialize(InGameRules inGame, InPauseRules inPause)
    {
        states = new Dictionary<State, IState>();
        states.Add(State.Pause, inPause);
        states.Add(State.Game, inGame);

        currentIState = states[State.Game];
    }

    public void SetState(State newState) {

        if(newState != currentState)
        {
            IState state = states[newState];

            currentIState.Exit();
            currentIState = state;
            currentIState.Enter();
        }
    }
    public void Tick()
    {
        currentIState.Tick();
    }
}
