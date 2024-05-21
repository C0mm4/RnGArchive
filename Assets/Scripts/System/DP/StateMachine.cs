using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StateMachine
{
    [SerializeField]
    private State currentState = null;

    [SerializeField]
    private Mob target;
    [SerializeField]
    private PlayerController playerT;

    private bool isPlayer;

    public StateMachine(Mob chr)
    {
        target = chr;
        isPlayer = false;
    }

    public StateMachine(PlayerController chr)
    {
        playerT = chr;
        isPlayer = true;
        setIdle();
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
            currentState = newState;

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
//        setIdle();
    }

    // set current state on idle and state stack empty
    public void setIdle()
    {
        if(playerT != null)
            changeState(new Idle());
        else
        {
            changeState(new MobIdle());
        }
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
