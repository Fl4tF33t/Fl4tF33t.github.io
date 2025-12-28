using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AOEAttack : ActionNode
{
    private bool endAnim;

    protected override void OnStart()
    {
        endAnim = false;

        // Check if the attack type is AOE
        if (context.frogBrain.attackType == LogicSO.AttackType.AOE)
        {
            // Subscribe to animation events
            context.animationEvents.OnEndAnim += AnimationEvents_OnEndAnim;
            context.animationEvents.OnDamageLogic += AnimationEvents_OnDamageLogic;

            // Trigger the attack animation
            context.animator.SetTrigger("OnAttack");
        }
    }

    // Method called when the animation triggers damage logic
    private void AnimationEvents_OnDamageLogic()
    {
        // Perform AOE damage and effects
        RaycastHit[] hits = Physics.SphereCastAll(context.transform.position, 0.5f, context.transform.forward, context.frogBrain.frogSO.logicSO.range, LayerMask.GetMask("Bug"));

        // Iterate through the hits array to process each hit
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.TryGetComponent(out IBugTakeDamage obj))
            {
                obj.BugTakeDamage(context.frogBrain.frog.damage);

                // Apply additional effects for Desert frogs
                if (context.gameObject.name.Contains("Desert"))
                {
                    obj.BugSlow();
                }
            }
        }
    }

    // Method called when the animation ends
    private void AnimationEvents_OnEndAnim()
    {
        endAnim = true; // Set the endAnim flag to true
    }

    protected override void OnStop()
    {
        endAnim = false; // Reset the endAnim flag when the node stops

        // Unsubscribe from animation events if the attack type is AOE
        if (context.frogBrain.attackType == LogicSO.AttackType.AOE)
        {
            context.animationEvents.OnEndAnim -= AnimationEvents_OnEndAnim;
            context.animationEvents.OnDamageLogic -= AnimationEvents_OnDamageLogic;
        }
    }

    protected override State OnUpdate()
    {
        // Check if the attack type is not AOE
        if (context.frogBrain.attackType != LogicSO.AttackType.AOE)
        {
            return State.Failure; // If not, return Failure
        }
        // Keep checking for when the visual animation is done
        if (!endAnim)
        {
            return State.Running; // If not done, return Running
        }
        return State.Success; // If animation is done, return Success
    }
}
