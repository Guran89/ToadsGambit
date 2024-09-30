using UnityEngine;
using UnityEngine.AI;

public class FlyController : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;
    public float playerDetectionRadius = 5f;
    public float fleeDistance = 15f;
    public float fleeSpeed = 8f;
    public float normalSpeed = 3f;

    private NavMeshAgent agent;
    private Animator animator;
    private float timer;
    private Transform player;

    private enum FlyState
    {
        Idle,
        Flying,
        Fleeing
    }

    private FlyState currentState = FlyState.Idle;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        timer = wanderTimer;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Enable flying
        agent.baseOffset = 4.5f; // Adjust this value to set the fly's height
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

        UpdateAnimation();
    }

    void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.speed = normalSpeed;
            agent.SetDestination(newPos);
            timer = 0;
            currentState = FlyState.Flying;
        }

        if (agent.remainingDistance < 0.1f)
        {
            currentState = FlyState.Idle;
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
            currentState = FlyState.Fleeing;
        }
    }

    Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    void UpdateAnimation()
    {
        switch (currentState)
        {
            case FlyState.Idle:
                animator.SetBool("isFlying", false);
                break;
            case FlyState.Flying:
            case FlyState.Fleeing:
                animator.SetBool("isFlying", true);
                break;
        }
    }
}