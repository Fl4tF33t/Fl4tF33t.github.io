using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class EnemySelection : ActionNode
{
    protected override void OnStart()
    {
        blackboard.selectedTarget = SelectTarget();
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        // Check if a target is selected
        if (blackboard.selectedTarget != null)
        {
            return State.Success; // If a target is selected, return Success
        }
        else
        {
            return State.Failure; // If no target is selected, return Failure
        }
    }

    // Method to select a target based on logic
    private GameObject SelectTarget()
    {
        GameObject target = null;

        // Choose a target based on the logic defined in the frog's brain
        switch (context.frogBrain.frog.target)
        {
            case LogicSO.Target.First:
                // Select the first target in line of sight
                if (blackboard.collidersInLOS.Count > 0)
                {
                    target = blackboard.collidersInLOS[0].gameObject;
                }
                break;

            case LogicSO.Target.Last:
                // Select the last target in line of sight
                int lastIndex = blackboard.collidersInLOS.Count - 1;
                if (lastIndex >= 0)
                {
                    target = blackboard.collidersInLOS[lastIndex].gameObject;
                }
                break;

            case LogicSO.Target.Strongest:
                // Select the target with the highest health
                float maxHealth = float.MinValue;
                foreach (Collider col in blackboard.collidersInLOS)
                {
                    BugBrain bugBrain = col.GetComponent<BugBrain>();
                    if (bugBrain != null && bugBrain.health > maxHealth)
                    {
                        maxHealth = bugBrain.health;
                        target = col.gameObject;
                    }
                }
                break;

            case LogicSO.Target.Weakest:
                // Select the target with the lowest health
                float minHealth = float.MaxValue;
                foreach (Collider col in blackboard.collidersInLOS)
                {
                    BugBrain bugBrain = col.GetComponent<BugBrain>();
                    if (bugBrain != null && bugBrain.health < minHealth)
                    {
                        minHealth = bugBrain.health;
                        target = col.gameObject;
                    }
                }
                break;

            case LogicSO.Target.Shield:
                // Select the target with the highest shield
                float maxShield = float.MinValue;
                foreach (Collider col in blackboard.collidersInLOS)
                {
                    BugBrain bugBrain = col.GetComponent<BugBrain>();
                    if (bugBrain != null && bugBrain.shield > maxShield)
                    {
                        maxShield = bugBrain.shield;
                        target = col.gameObject;
                    }
                }
                break;
        }

        return target; 
    }
}
