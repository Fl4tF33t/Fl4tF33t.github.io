using UnityEngine;
using UnityEngine.AI;

public class PatrolSystem : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetDestination();
    }

    void Update()
    {
        // Check if the agent has reached the destination
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            // Set the next destination
            SetDestination();
        }
    }

    void SetDestination()
    {
        if (patrolPoints.Length == 0)
        {
            Debug.LogError("No patrol points assigned to the Patrol System!");
            return;
        }

        // Set the destination to the current patrol point
        agent.destination = patrolPoints[currentPatrolIndex].position;

        // Increment the index for the next patrol point
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    // Draw gizmos to visualize patrol points
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (Transform point in patrolPoints)
        {
            Gizmos.DrawSphere(point.position, 0.2f);
        }
    }
}
