using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class Jump : ActionNode
{
    public float rotationThreshold = 1.0f; // The threshold angle for considering the rotation as correct
    private bool isRotationCorrect;

    public float distanceThreshold = 0.05f; // The threshold distance for considering the target reached
    public float moveSpeed = 2f; // The movement speed

    private bool isJumpAnimStart;

    protected override void OnStart()
    {
        context.agent.enabled = false; // Disable the NavMeshAgent to allow manual movement

        isJumpAnimStart = false;
        isRotationCorrect = false;
    }

    protected override void OnStop()
    {
        context.agent.enabled = true; // Re-enable the NavMeshAgent
    }

    // Track the target position and rotate towards it
    private void Track(Vector3 position)
    {
        Vector3 targetDirection = position - context.transform.position;
        targetDirection.y = 0; // Restrict rotation to the XZ plane

        // Calculate the desired rotation based on the direction
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Apply the rotation only on the Y-axis
        context.transform.rotation = Quaternion.RotateTowards(context.transform.rotation, targetRotation, rotationThreshold);

        // Check if the rotation is correct
        float angleDifference = Quaternion.Angle(context.transform.rotation, targetRotation);
        isRotationCorrect = (angleDifference <= rotationThreshold);
    }

    protected override State OnUpdate()
    {
        // Calculate the direction to the target and rotate towards it if rotation is not correct
        if (!isRotationCorrect)
        {
            Track(blackboard.jumpLocation);
        }

        // Start the jump animation if rotation is correct and not already started
        if (!isJumpAnimStart && isRotationCorrect)
        {
            context.animator.SetBool("OnJump", true);
            isJumpAnimStart = true;
        }

        // If rotation is correct, move towards the target
        if (isRotationCorrect)
        {
            Vector3 direction = (blackboard.jumpLocation - context.transform.position).normalized;
            float distanceToTarget = Vector3.Distance(context.transform.position, blackboard.jumpLocation);

            // If the object is far from the target, move towards it
            if (distanceToTarget > distanceThreshold)
            {
                context.transform.position += direction * moveSpeed * Time.deltaTime;
            }
            else
            {
                // If the object is close enough to the target, consider it reached
                context.animator.SetBool("OnJump", false);
                return State.Success;
            }
        }

        return State.Running;
    }
}
