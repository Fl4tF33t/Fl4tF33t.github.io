using UnityEngine;

public class Luring : BaseState
{
    private FrogSM sm;

    /*private bool grounded;
    private int groundedLayer = 1 << 6;*/

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