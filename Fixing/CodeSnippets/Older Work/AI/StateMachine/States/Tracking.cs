using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tracking : BaseState
{
    private FrogSM sm;
    private FrogData fd;

    private float attackTimer;

    // Constructor for the Tracking state
    public Tracking(FrogSM stateMachine) : base("Tracking", stateMachine)
    {
        sm = (FrogSM)this.stateMachine;
    }

    // Called when entering the Tracking state
    public override void Enter()
    {
        base.Enter();
        fd = sm.frogData;

        // Check if the frog is not detecting any targets; if so, transition to idling state
        if (!fd.GetCirCol2dDetection().IsTouchingLayers(sm.frogData.trackingLayer))
        {
            stateMachine.ChangeState(sm.idlingState);
            return;
        }

        // Calculate the attack timer based on fire rate and buffs
        attackTimer = 1 / (fd.GetFireRate() + FrogBuff());

        // Subscribe to the event for targets changed
        fd.OnTargetsChanged += Fd_OnTargetsChanged;

        // Set the baseBugInfo based on the attack type and overlapping colliders
        if (fd.GetOverlappingColliders2D().Count > 0)
        {
            switch (fd.attackType)
            {
                case FrogData.AttackType.First:
                    fd.baseBugInfo = fd.GetOverlappingColliders2D()[0].GetComponent<BaseBug>();
                    break;
                case FrogData.AttackType.Last:
                    fd.baseBugInfo = fd.GetOverlappingColliders2D()[fd.GetOverlappingColliders2D().Count - 1].GetComponent<BaseBug>();
                    break;
            }
        }
    }

    // Calculate buffs based on sunglasses and fire rate
    float FrogBuff()
    {
        if (fd.sunglassesBuff)
        {
            float percentageBuff = 0;
            foreach (float number in fd.fireRateBuff)
            {
                if (number > percentageBuff)
                {
                    percentageBuff = number;
                }
            }
            float currentFireRate = fd.GetFireRate();

            float fireRateIncrease = (currentFireRate * fd.GetFireRate()) / 100f;

            return fireRateIncrease;
        }
        else
            return 0;
    }

    // Called when the targets change
    private void Fd_OnTargetsChanged(object sender, System.EventArgs e)
    {
        // If the frog is still touching the tracking layer, update the baseBugInfo
        if (fd.GetCirCol2dDetection().IsTouchingLayers(6))
        {
            switch (fd.attackType)
            {
                case FrogData.AttackType.First:
                    fd.baseBugInfo = fd.GetOverlappingColliders2D()[0].GetComponent<BaseBug>();
                    break;
                case FrogData.AttackType.Last:
                    fd.baseBugInfo = fd.GetOverlappingColliders2D()[fd.GetOverlappingColliders2D().Count - 1].GetComponent<BaseBug>();
                    break;
            }
        }
        else
            stateMachine.ChangeState(sm.idlingState);
    }

    // Called every frame for logic updates
    public override void UpdateLogic()
    {
        base.UpdateLogic();

        // Check if the frog is not detecting any targets; if so, transition to idling state
        if (!fd.GetCirCol2dDetection().IsTouchingLayers(sm.frogData.trackingLayer))
        {
            stateMachine.ChangeState(sm.idlingState);
        }

        // Update tracking logic
        TrackingBugs();
    }

    // Called every frame for physics updates
    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

    }

    // Called when exiting the Tracking state
    public override void Exit()
    {
        // Unsubscribe from the event when exiting the state
        fd.OnTargetsChanged -= Fd_OnTargetsChanged;
    }

    // Update the rotation and handle attacks based on overlapping colliders
    private void TrackingBugs()
    {
        if (fd.GetOverlappingColliders2D().Count > 0)
        {
            // Calculate direction and rotation
            Vector3 direction = fd.GetOverlappingColliders2D()[0].transform.position - sm.transform.position;
            Vector3 directionOffset = direction + (fd.GetOverlappingColliders2D()[0].transform.up * fd.baseBugInfo.bugSO.speed * 0.5f);
            float angle = Mathf.Atan2(directionOffset.y, directionOffset.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            // Smoothly rotate the frog towards the target
            sm.transform.rotation = Quaternion.Slerp(sm.transform.rotation, Quaternion.Euler(rotation.eulerAngles), fd.GetTrackingSpeed() * Time.deltaTime);

            // Update the attack timer
            attackTimer -= Time.deltaTime;

            // Check for conditions to transition to the attacking state
            if (attackTimer < 0 && Quaternion.Angle(rotation, sm.transform.rotation) < 6 * fd.GetTrackingSpeed())
            {
                stateMachine.ChangeState(sm.attackingState);
            }
        }
        else
            stateMachine.ChangeState(sm.idlingState);
    }
}
