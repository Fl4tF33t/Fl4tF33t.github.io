using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class TrackEnemy : ActionNode
{
    public float rotationThreshold = 1.0f; // The threshold angle for considering the rotation as correct
    private bool isRotationCorrect = false;

    protected override void OnStart()
    {
        // Set the attack timer to the time it takes to shoot a target if it's not already set
        if (blackboard.attackTimer <= 0)
        {
            blackboard.attackTimer = 1f / context.frogBrain.frogSO.logicSO.attackSpeed;
        }

        // Trigger the tracking animation if not already in tracking state
        if (!context.animator.GetCurrentAnimatorStateInfo(0).IsName("OnTracking"))
        {
            context.animator.SetTrigger("OnTracking");
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        // Decrease the attack timer
        blackboard.attackTimer -= Time.deltaTime;

        // Track the selected target if it exists
        if (blackboard.selectedTarget != null)
        {
            Track(blackboard.selectedTarget);
        }

        // Check conditions for success, failure, or continue running
        if (blackboard.attackTimer <= 0 && isRotationCorrect)
        {
            return State.Success;
        }

        if (blackboard.selectedTarget == null || !blackboard.selectedTarget.activeSelf)
        {
            return State.Failure;
        }

        float dist = Vector3.Distance(context.transform.position, blackboard.selectedTarget.transform.position);
        if (dist > context.frogBrain.frog.range)
        {
            return State.Failure;
        }

        if (Physics.Linecast(context.transform.position, blackboard.selectedTarget.transform.position, LayerMask.GetMask("BlockLOS")))
        {
            return State.Failure;
        }

        return State.Running;
    }

    // Track the target GameObject
    private void Track(GameObject target)
    {
        Vector3 targetDirection = target.transform.position - context.transform.position;
        targetDirection.y = 0; // Restrict rotation to the XZ plane

        // Calculate the desired rotation based on the direction
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Apply the rotation only on the Y-axis
        context.transform.rotation = Quaternion.RotateTowards(context.transform.rotation, targetRotation, 200f * Time.deltaTime);

        // Check if the rotation is correct
        float angleDifference = Quaternion.Angle(context.transform.rotation, targetRotation);
        isRotationCorrect = (angleDifference <= rotationThreshold);
    }

}
