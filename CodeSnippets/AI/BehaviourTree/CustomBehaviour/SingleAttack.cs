using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using System;

public class SingleAttack : ActionNode
{
    private bool endAnim; // Flag to track if the animation has ended
    private float tongueDistance = 9f; // Distance multiplier for the tongue extension
    private float tongueSpeed = 100f; // Speed of tongue extension

    protected override void OnStart()
    {
        endAnim = false;

        if (context.frogBrain.attackType == LogicSO.AttackType.Single)
        {
            // Calculate direction and distance to the selected target
            Vector3 direction = blackboard.selectedTarget.transform.position - context.transform.position;
            float distance = direction.magnitude;
            // Start coroutine for tongue extension animation
            context.frogBrain.StartCoroutine(Tongue(distance * tongueDistance, tongueSpeed));
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        // Check if the attack type is not Single
        if (context.frogBrain.attackType != LogicSO.AttackType.Single)
        {
            return State.Failure;
        }

        // Check if no selected target
        if (blackboard.selectedTarget == null)
        {
            return State.Failure;
        }

        // Check if the animation has ended
        if (endAnim)
        {
            return State.Success;
        }

        return State.Running;
    }

    // Coroutine for extending the tongue
    private IEnumerator Tongue(float maxScale, float scaleSpeed)
    {
        while (maxScale > context.frogBrain.singleAttack.localScale.y && blackboard.selectedTarget != null)
        {
            context.frogBrain.singleAttack.localScale += Vector3.up * Time.deltaTime * scaleSpeed;
            yield return null;
        }
        // After tongue extension, start shrinking coroutine
        context.frogBrain.StartCoroutine(Shrink(scaleSpeed));
    }

    // Coroutine for shrinking the tongue
    private IEnumerator Shrink(float scaleSpeed)
    {
        while (context.frogBrain.singleAttack.localScale.y >= .99)
        {
            context.frogBrain.singleAttack.localScale += Vector3.down * Time.deltaTime * scaleSpeed;
            yield return null;
        }
        // If target exists, apply damage to it
        if (blackboard.selectedTarget != null)
        {
            blackboard.selectedTarget.GetComponent<IBugTakeDamage>().BugTakeDamage(context.frogBrain.frog.damage);
        }
        endAnim = true; // Set endAnim flag to true
    }
}
