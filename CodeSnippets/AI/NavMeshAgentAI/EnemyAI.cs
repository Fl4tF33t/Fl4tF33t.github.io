using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform player;
    public float detectionRadius = 10f;
    public float attackRange = 2f;
    public float fieldOfViewAngle = 90f;
    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private bool isPlayerDetected = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetNextWaypoint();
    }

    void Update()
    {
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
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            SetNextWaypoint();
        }
    }

    void SetNextWaypoint()
    {
        agent.destination = waypoints[currentWaypointIndex].position;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    bool PlayerWithinDetectionRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return distanceToPlayer <= detectionRadius;
    }

    bool PlayerWithinAttackRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return distanceToPlayer <= attackRange;
    }

    void ChasePlayer()
    {
        agent.destination = player.position;
    }

    void AttackPlayer()
    {
        // Attack logic goes here
        Debug.Log("Attacking player!");
    }

    // Visualize detection radius and field of view in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Vector3 leftDirection = Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * transform.forward;
        Vector3 rightDirection = Quaternion.Euler(0, fieldOfViewAngle / 2, 0) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + leftDirection * detectionRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightDirection * detectionRadius);
    }
}
