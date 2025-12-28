using UnityEngine;

public class Idling : BaseState
{
    //this state, nothing is happening and usually is used before the game starts, most other states will go through idle quickly if nothing is done
    private FrogSM sm;
    private FrogData fd;

    private float jumpTimer;
    private float lastJumpTimer;

    public Idling(FrogSM stateMachine) : base("Idling", stateMachine) 
    {
        sm = (FrogSM)this.stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        fd = sm.frogData;

        if (lastJumpTimer <= 0)
        {
            jumpTimer = 10f;
        }
        else jumpTimer = lastJumpTimer;
    }

    public override void UpdateLogic()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(sm.trackingState);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            stateMachine.ChangeState(sm.jumpingState);
        }

        if (sm.frogData.GetCirCol2dDetection().IsTouchingLayers(sm.frogData.trackingLayer))
        {
            stateMachine.ChangeState(sm.trackingState);
        }else RandomJump();
    }


    public override void Exit()
    {
        lastJumpTimer = jumpTimer;
    }


    //based on a time interval, a dice is thrown to see if the AI frog will jump
    //the chance of jumping is reduced with a higher disciplin level indicated by chance
    //the timer resets after each test
    private void RandomJump()
    {
        jumpTimer -= Time.deltaTime;

        if (jumpTimer < 0)
        {
            if (RandomJumpTest(sm.frogData.GetDisciplin()))
            {
                stateMachine.ChangeState(sm.jumpingState);
            }
        }
    }
    private bool RandomJumpTest(int chance)
    {
        int randomIndex = Random.Range(1, 7);

        if (randomIndex <= chance)
        {
            jumpTimer = 10f;
            return false;
        }
        else return true;
    }


}