using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    //base that all the states will inherit from to create each state
    //here there will be a name for each state allong with which statemachine it is implemented into
    //the logic and physics has been seperated to influence each indirectly to help with visuals as well

    public string name;

    protected StateMachine stateMachine;

    public BaseState(string name, StateMachine stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }
}