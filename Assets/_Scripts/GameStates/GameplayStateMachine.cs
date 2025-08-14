using System.Collections.Generic;
using Zenject;

public class GameplayStateMachine
{
    private State currentState = State.Initialize;
    private IState currentIState;
    private Dictionary<State, IState> states;

    [Inject]
    private void Initialize(InInitializeRules isInit, InGameRules inGame, InPauseRules inPause, InEndRules inEnd)
    {
        states = new Dictionary<State, IState>();
        states.Add(State.Initialize, isInit);
        states.Add(State.Pause, inPause);
        states.Add(State.Game, inGame);
        states.Add(State.End, inEnd);

        //State.Initialize
        currentIState = states[currentState];
        currentIState.Enter();
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
