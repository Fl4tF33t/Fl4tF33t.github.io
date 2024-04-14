using UnityEngine;

public class Luring : BaseState
{
    //new empty state that requires logic
    private FrogSM sm;

    public Luring(FrogSM stateMachine) : base("Luring", stateMachine)
    {
        sm = (FrogSM)this.stateMachine;
    }

    public override void Enter()
    {
        
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        //reaches destination then goes back to the original idle state
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void Exit()
    {
        base.Exit();
    }
}