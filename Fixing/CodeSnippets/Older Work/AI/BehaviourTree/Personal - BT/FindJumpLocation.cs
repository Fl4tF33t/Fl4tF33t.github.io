using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using static FrogBrain;
using UnityEngine.AI;

public class FindJumpLocation : ActionNode
{
    private Vector3 jumpLocation;

    protected override void OnStart()
    {
        // Generate a random jump location on the edge of a circle around the agent
        jumpLocation = RandomPointOnCircleEdge(context.transform.position, 4f);

        // Keep generating a new jump location until it is valid
        while (!IsValidLocation(jumpLocation))
        {
            jumpLocation = RandomPointOnCircleEdge(context.transform.position, 4f);
        }

        // Assign the valid jump location to the blackboard
        blackboard.jumpLocation = jumpLocation;
    }

    protected override void OnStop()
    {
    }

    // Generate a random point on the edge of a circle
    private Vector3 RandomPointOnCircleEdge(Vector3 center, float radius)
    {
        // Generate a random angle around the circle in radians
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        // Calculate the position of the point on the circle edge
        float x = center.x + radius * Mathf.Cos(randomAngle);
        float z = center.z + radius * Mathf.Sin(randomAngle);

        // Set the y-coordinate to match the top-down view
        float y = center.y;

        return new Vector3(x, y, z);
    }

    // Check if a location is valid for jumping
    private bool IsValidLocation(Vector3 pos)
    {
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(pos, out navHit, 0.01f, NavMesh.AllAreas))
        {
            // Check if there are other game objects that would collide within the same area
            Collider[] colliders = Physics.OverlapSphere(navHit.position, 0.2f);

            // Check if the sampled area corresponds to ground frogs or water frogs based on the mask
            switch (navHit.mask)
            {
                case 1: // Ground frogs
                    return !context.frogBrain.frogSO.logicSO.isWaterFrog && colliders.Length == 0;
                case 8: // Path for bugs (not a valid location)
                    return false;
                case 16: // Water frogs
                    return context.frogBrain.frogSO.logicSO.isWaterFrog && colliders.Length == 0;
                default:
                    return false;
            }
        }
        return false;
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}
