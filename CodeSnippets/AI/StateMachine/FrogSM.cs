using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FrogSM : StateMachine
{ 
    //stripped down, reusable statemachine layout for future AI

    [SerializeField]
    private FrogSO frogSO;
    [HideInInspector]
    public FrogSO currentFrogSO 
    { 
        get
        {
            return frogSO;
        }
        private set
        {
            currentFrogSO = value;
        }
    }
    
    //all possible states for the frog agent
    [HideInInspector]
    public Idling idlingState;
    [HideInInspector]
    public Tracking trackingState;
    [HideInInspector]
    public Attacking attackingState;
    [HideInInspector]
    public Jumping jumpingState;
    [HideInInspector]
    public Luring luringState;

    private void Awake()
    {
        //initialising each state
        idlingState = new Idling(this);
        trackingState = new Tracking(this);
        attackingState = new Attacking(this);
        jumpingState = new Jumping(this);
        luringState = new Luring(this);
    }

    protected override BaseState GetInitialState()
    { 
        return idlingState;
    }
}