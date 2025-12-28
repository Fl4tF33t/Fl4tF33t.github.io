using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachine : MonoBehaviour
{
    //the base StateMachine that will be inherited for each statemachine that is used
    //this is the monobehaviour that is attached to the game object
    //event system used to send out notifications for any music, animations, etc. that need to be done
    public event EventHandler<OnAgentStateEventArgs> OnAgentState;
    public class OnAgentStateEventArgs : EventArgs
    {
        public BaseState agentState;
    }

    BaseState currentState;

    void OnEnable()
    {
        currentState = GetInitialState();
        if (currentState != null)
            currentState.Enter();
        OnAgentState?.Invoke(this, new OnAgentStateEventArgs
        {
            agentState = currentState
        }); ;
    }

    void Update()
    {
        if (currentState != null)
            currentState.UpdateLogic();
    }

    void LateUpdate()
    {
        if (currentState != null)
            currentState.UpdatePhysics();
    }

    protected virtual BaseState GetInitialState()
    {
        return null;
    }

    public void ChangeState(BaseState newState)
    {
        currentState.Exit();

        currentState = newState;
        newState.Enter(); 
        OnAgentState?.Invoke(this, new OnAgentStateEventArgs
        {
            agentState = currentState
        }); ;
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(50f, 500f, 200f, 100f));
        string content = currentState != null ? currentState.name : "(no current state)";
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
        GUILayout.EndArea();

    }
}