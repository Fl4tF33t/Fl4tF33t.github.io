using UnityEngine;
using UnityEngine.AI;

public class AgentDetector : MonoBehaviour
{
    public float detectionRadius = 5f; // Radius within which the agent can detect targets
    public float fieldOfViewAngle = 90f; // Field of view angle for the agent
    public LayerMask targetMask; // Layer mask for targets
    private NavMeshAgent agent;
    private Transform target;
    private bool isTargetDetected = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Detect target
        DetectTarget();
    }

    void DetectTarget()
    {
        Collider[] targetsInRadius = Physics.OverlapSphere(transform.position, detectionRadius, targetMask);

        foreach (Collider potentialTarget in targetsInRadius)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - transform.position;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            // Check if target is within field of view
            if (angleToTarget < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToTarget, out hit, detectionRadius))
                {
                    if (hit.collider.CompareTag("Target"))
                    {
                        // Target detected
                        target = potentialTarget.transform;
                        isTargetDetected = true;
                        Debug.Log("Target detected: " + target.name);
                        // Additional logic here (e.g., attack, pursue, etc.)
                        return;
                    }
                }
            }
        }

        // No target detected
        target = null;
        isTargetDetected = false;
    }

    // Visualize detection radius in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Draw field of view angle
        Vector3 leftDirection = Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * transform.forward;
        Vector3 rightDirection = Quaternion.Euler(0, fieldOfViewAngle / 2, 0) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + leftDirection * detectionRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightDirection * detectionRadius);
    }
}
