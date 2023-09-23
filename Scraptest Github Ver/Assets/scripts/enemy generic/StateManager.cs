using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    // tutorial: https://www.youtube.com/watch?v=cnpJtheBLLY
    public State currentState;

    void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState(); //check if currentstate is null or not, ignore if null
        
        if (nextState!= null )
        {
            // switch to next stage
            SwitchToTheNextState(nextState);
        }

    }

    private void SwitchToTheNextState(State nextState)
    {
        currentState = nextState;
    }
}
