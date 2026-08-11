using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckEnemyInArea : ActionNode
{
    protected override void OnStart()
    {
        // Find all colliders in the area around the agent and assign them to the blackboard
        blackboard.collidersInArea = Physics.OverlapSphere(context.transform.position, context.frogBrain.frog.range, context.frogBrain.frogSO.logicSO.targetLayer);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        // Check if there are any colliders in the area
        if (blackboard.collidersInArea.Length > 0)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
