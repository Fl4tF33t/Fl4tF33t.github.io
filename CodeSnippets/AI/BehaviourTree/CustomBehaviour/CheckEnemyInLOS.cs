using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckEnemyInLOS : ActionNode
{
    protected override void OnStart()
    {
        // Clear the list of colliders in line of sight when the action starts
        blackboard.collidersInLOS.Clear();
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        // Check line of sight to each collider in the area
        foreach (Collider item in blackboard.collidersInArea)
        {
            // Check if there is an obstruction between the agent and the collider
            if (Physics.Linecast(context.transform.position, item.transform.position, LayerMask.GetMask("BlockLOS")))
            {
                continue;
            }
            else
            {
                // If there is no obstruction, add the collider to the list of colliders in line of sight
                blackboard.collidersInLOS.Add(item);
            }
        }

        // Check if there are any colliders in line of sight
        if (blackboard.collidersInLOS.Count > 0)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
