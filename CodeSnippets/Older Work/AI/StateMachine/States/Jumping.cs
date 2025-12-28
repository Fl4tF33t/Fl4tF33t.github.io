using UnityEngine;
using UnityEngine.AI;

public class Jumping : BaseState
{
    private FrogSM sm;
    private FrogData fd;

    Vector2 direction;
    Vector2 randomDestination;
    Quaternion rotation;
    bool isJumping;
    public Jumping(FrogSM stateMachine) : base("Jumping", stateMachine)
    {
        sm = (FrogSM)this.stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        fd = sm.frogData;

        //Find a free spot to jump to, then jump to that spot
        while (!RandomPointOnCircleEdge(sm.transform.position, 5, out randomDestination))
        {
            RandomPointOnCircleEdge(sm.transform.position, 5, out randomDestination);
        }

        //Sets the direction that the frog will rotate towards
        direction = new Vector3(randomDestination.x, randomDestination.y, 0) - sm.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotation = Quaternion.AngleAxis(angle - 95, Vector3.forward);

        //turn of Nav<meshAgent to use basic movement 
        fd.GetNavMeshAgent().enabled = false;
    }

    public override void UpdateLogic()
    {
        float speed = 2 * Time.deltaTime;
        //rotation of the agent unsing Quaternion slerp
        sm.transform.rotation = Quaternion.Slerp(sm.transform.rotation, Quaternion.Euler(rotation.eulerAngles), fd.GetTrackingSpeed() * Time.deltaTime);
        if (Quaternion.Angle(rotation, sm.transform.rotation) < 0.1f)
        {
            //start jumping animation
            if (!isJumping)
            {
                isJumping = true;
                sm.StartJumpAnimation(isJumping);
            }
            if (isJumping)
            {
                //Once rotation is complete, jumping will start
                sm.transform.position = Vector2.MoveTowards(sm.transform.position, randomDestination, speed);
                if (sm.transform.position.x == randomDestination.x && sm.transform.position.y == randomDestination.y)
                {
                    stateMachine.ChangeState(sm.idlingState);
                } 
            }
        }
    }

    public override void Exit()
    {
        //Turning the navmesh back on when the location is reached
        fd.GetNavMeshAgent().enabled = true;
        isJumping = false;
        sm.StartJumpAnimation(isJumping);

    }

    private bool RandomPointOnCircleEdge(Vector2 center, float radius, out Vector2 result)
    {
        // Generate a random point within the circle
        float randomAngle = Random.Range(0f, 360f); // Random angle in degrees
        float randomRadians = randomAngle * Mathf.Deg2Rad; // Convert to radians
        float randomRadius = Random.Range(0f, radius);

        float x = center.x + randomRadius * Mathf.Cos(randomRadians);
        float y = center.y + randomRadius * Mathf.Sin(randomRadians);

        Vector3 pos = new Vector3(x, y, 0f);

        // Find the nearest valid point on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, 1f, NavMesh.AllAreas))
        {
            if (IsPathAvailable(sm.transform.position, hit.position))
            {
                result = new Vector2(hit.position.x, hit.position.y);
                return true;
            }
        }
        result = Vector2.zero;
        return false;
    }


    private bool IsPathAvailable(Vector3 startPos, Vector3 endPos)
    {
        NavMeshPath path = new NavMeshPath();
        bool hasPath = NavMesh.CalculatePath(startPos, endPos, NavMesh.AllAreas, path);

        if (hasPath && path.status == NavMeshPathStatus.PathComplete)
        {
            return true; // A valid path exists
        }
        return false; // No valid path exists
    }

}
