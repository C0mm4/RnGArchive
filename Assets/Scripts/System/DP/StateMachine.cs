using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private State currentState = null;

    private KinematicObject target;
    private PlayerController playerT;

    private bool isPlayer;

    public StateMachine(KinematicObject chr)
    {
        target = chr;
        isPlayer = false;
    }

    public StateMachine(PlayerController chr)
    {
        playerT = chr;
        isPlayer = true;
    }

    // change charator state
    public void changeState(State newState)
    {
        if(newState == null)
        {
            currentState = null;
        }
        // check this first regist state
        else if (currentState != null) {
            // check FSM
            if (GameManager.FSM.getList(currentState.GetType().Name).Contains(newState.GetType().Name))
            {
                // past state add to stack, new state regist on currentstate
                if (currentState.GetType().Name != newState.GetType().Name)
                {
                    // change New State
                    currentState = newState;

                    // State Enter Logic Process
                    if (isPlayer)
                    {
                        currentState.EnterState(playerT);
                    }
                    else
                    {
                        currentState.EnterState(target);
                    }
                }
                // if currentState is Equal State
                else
                {
                    // change New State
                    currentState = newState;

                    // State Enter Logic Process
                    if (isPlayer)
                    {
                        currentState.EnterState(playerT);
                    }
                    else
                    {
                        currentState.EnterState(target);
                    }
                }
            }
        }
        else
        {
            Debug.Log("Initialize");
            currentState = newState;

            if (isPlayer)
            {
                currentState.EnterState(playerT);

                Debug.Log("Initialize");
            }
            else
            {
                currentState.EnterState(target);
            }
        }
    }

    // state update on frame
    public void updateState()
    {
        if(currentState != null )
        {
            currentState.UpdateState();
        }
    }

    // exit currentstate
    public void exitState()
    {
        if(currentState != null)
            currentState.ExitState();
        setIdle();
    }

    // set current state on idle and state stack empty
    public void setIdle()
    {
        changeState(new Idle());
    }
    // return current state name to string
    public string getStateStr()
    {
        return currentState.GetType().Name;
    }
    // return current state
    public State getState()
    {
        return currentState;
    }
}
