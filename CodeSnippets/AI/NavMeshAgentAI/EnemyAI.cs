using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoints for patrolling
    public Transform player; // Reference to the player
    public float detectionRadius = 10f; // Radius within which the enemy detects the player
    public float attackRange = 2f; // Range within which the enemy attacks the player
    public float fieldOfViewAngle = 90f; // Field of view angle for player detection
    private int currentWaypointIndex = 0; // Index of the current waypoint
    private NavMeshAgent agent; // Reference to the NavMeshAgent component
    private bool isPlayerDetected = false; // Flag to indicate if the player is detected

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component attached to this GameObject
        SetNextWaypoint(); // Set the initial destination to the first waypoint
    }

    void Update()
    {
        // Check if the player is within detection range
        if (PlayerWithinDetectionRange())
        {
            isPlayerDetected = true;
            ChasePlayer();

            if (PlayerWithinAttackRange())
            {
                AttackPlayer();
            }
        }
        else
        {
            isPlayerDetected = false;
            Patrol();
        }
    }

    void Patrol()
    {
        // Check if the agent has reached the current waypoint
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            SetNextWaypoint(); // Set the next waypoint as the destination
        }
    }

    void SetNextWaypoint()
    {
        agent.destination = waypoints[currentWaypointIndex].position; // Set the destination to the position of the current waypoint
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Move to the next waypoint in the array
    }

    bool PlayerWithinDetectionRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position); // Calculate the distance to the player
        return distanceToPlayer <= detectionRadius;
    }

    bool PlayerWithinAttackRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position); // Calculate the distance to the player
        return distanceToPlayer <= attackRange;
    }

    void ChasePlayer()
    {
        agent.destination = player.position; // Set the player's position as the destination
    }

    void AttackPlayer()
    {
        // Attack logic goes here
        Debug.Log("Attacking player!");
    }

    // Visualize detection radius and field of view in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Set the color of the gizmos to red
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Draw a wire sphere to visualize the detection radius

        // Calculate left and right directions based on the field of view angle
        Vector3 leftDirection = Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * transform.forward;
        Vector3 rightDirection = Quaternion.Euler(0, fieldOfViewAngle / 2, 0) * transform.forward;

        // Draw lines to visualize the field of view
        Gizmos.DrawLine(transform.position, transform.position + leftDirection * detectionRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightDirection * detectionRadius);
    }
}
