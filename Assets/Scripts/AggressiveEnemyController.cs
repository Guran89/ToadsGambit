using UnityEngine;
using UnityEngine.AI;

public class AggressiveEnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Patrolling,
        Chasing,
        Attacking,
        AttackCooldown
    }

    public float sightRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public float patrolWaitTime = 2f;
    public float patrolRange = 10f;
    public float damageAmount = 10f;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;
    private EnemyState currentState;
    private float lastAttackTime;
    private Vector3 patrolDestination;
    private float patrolWaitTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = EnemyState.Idle;
        SetNewPatrolDestination();
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdleState();
                break;
            case EnemyState.Patrolling:
                HandlePatrollingState();
                break;
            case EnemyState.Chasing:
                HandleChasingState();
                break;
            case EnemyState.Attacking:
                HandleAttackingState();
                break;
            case EnemyState.AttackCooldown:
                HandleAttackCooldownState();
                break;
        }

        UpdateAnimations();
    }

    void HandleIdleState()
    {
        if (CanSeePlayer())
        {
            currentState = EnemyState.Chasing;
        }
        else if (patrolWaitTimer <= 0)
        {
            currentState = EnemyState.Patrolling;
            SetNewPatrolDestination();
        }
        else
        {
            patrolWaitTimer -= Time.deltaTime;
        }
    }

    void HandlePatrollingState()
    {
        if (CanSeePlayer())
        {
            currentState = EnemyState.Chasing;
        }
        else if (agent.remainingDistance < 0.1f)
        {
            currentState = EnemyState.Idle;
            patrolWaitTimer = patrolWaitTime;
        }
    }

    void HandleChasingState()
    {
        if (!CanSeePlayer())
        {
            currentState = EnemyState.Idle;
            return;
        }

        agent.SetDestination(player.position);

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            currentState = EnemyState.Attacking;
        }
    }

    void HandleAttackingState()
    {
        agent.SetDestination(transform.position); // Stop moving
        transform.LookAt(player);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
            currentState = EnemyState.AttackCooldown;
        }
    }

    void HandleAttackCooldownState()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                currentState = EnemyState.Attacking;
            }
            else
            {
                currentState = EnemyState.Chasing;
            }
        }
    }

    void Attack()
    {
        // Implement damage to player here
        Debug.Log("Enemy attacks player for " + damageAmount + " damage!");
        // You would typically call a method on the player to apply damage
        // e.g., player.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
    }

    bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < sightRange)
        {
            RaycastHit hit;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, sightRange))
            {
                if (hit.transform == player)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void SetNewPatrolDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRange;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, patrolRange, 1);
        patrolDestination = hit.position;
        agent.SetDestination(patrolDestination);
    }

    void UpdateAnimations()
    {
        animator.SetBool("IsWalking", agent.velocity.magnitude > 0.1f && currentState != EnemyState.Chasing);
        animator.SetBool("IsRunning", currentState == EnemyState.Chasing);
        animator.SetBool("IsAttacking", currentState == EnemyState.Attacking);
        animator.SetBool("IsIdle", currentState == EnemyState.Idle || currentState == EnemyState.AttackCooldown);
    }
}