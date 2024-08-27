using UnityEngine;
using UnityEngine.AI;

public class EnemyWanderer : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;
    public float playerDetectionRadius = 5f;
    public float fleeDistance = 15f;
    public float fleeSpeed = 8f;

    private NavMeshAgent agent;
    private float timer;
    private Transform player;

    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= playerDetectionRadius)
        {
            FleeFromPlayer();
        }
        else
        {
            Wander();
        }
    }

    void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.speed = agent.speed / 2; // Slower speed for wandering
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    void FleeFromPlayer()
    {
        Vector3 fleeDirection = transform.position - player.position;
        Vector3 newPos = transform.position + fleeDirection.normalized * fleeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(newPos, out hit, fleeDistance, NavMesh.AllAreas))
        {
            agent.speed = fleeSpeed;
            agent.SetDestination(hit.position);
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}