using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ProjectileAttack : ActionNode
{
    private bool endAnim;

    protected override void OnStart()
    {
        endAnim = false;

        // Check if the attack type is Projectile
        if (context.frogBrain.attackType == LogicSO.AttackType.Projectile)
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
        // Spawn the projectile prefab at the specified position
        GameObject prefab = Instantiate(context.frogBrain.projectile, context.frogBrain.projectilePos.position, context.transform.rotation);
        // Set the damage value of the projectile
        prefab.GetComponent<ProjectileLogic>().damage = context.frogBrain.frog.damage;
    }

    // Method called when the animation ends
    private void AnimationEvents_OnEndAnim()
    {
        endAnim = true; // Set the endAnim flag to true
    }

    protected override void OnStop()
    {
        endAnim = false; // Reset the endAnim flag when the node stops

        // Unsubscribe from animation events if the attack type is Projectile
        if (context.frogBrain.attackType == LogicSO.AttackType.Projectile)
        {
            context.animationEvents.OnEndAnim -= AnimationEvents_OnEndAnim;
            context.animationEvents.OnDamageLogic -= AnimationEvents_OnDamageLogic;
        }
    }

    protected override State OnUpdate()
    {
        // Check if the attack type is not Projectile
        if (context.frogBrain.attackType != LogicSO.AttackType.Projectile)
        {
            return State.Failure; // If not, return Failure
        }
        // Keep checking for when the visual animation is done
        if (!endAnim)
        {
            return State.Running;
        }
        return State.Success;
    }
}
