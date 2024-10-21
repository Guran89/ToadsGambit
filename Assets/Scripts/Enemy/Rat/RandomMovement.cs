using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float patrolRange = 10f;
    [SerializeField] private float pauseDuration = 2f;
    [SerializeField] private Transform centrePoint;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private LayerMask playerLayer;

    private NavMeshAgent agent;
    private Transform player;
    private bool isFollowingPlayer;
    private Coroutine currentRoutine;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
        }
        currentRoutine = StartCoroutine(PatrolRoutine());
    }

    private void Update()
    {
        bool canSeePlayer = CanSeePlayer();

        // Visualize player detection
        Debug.DrawLine(transform.position, player.position, canSeePlayer ? Color.red : Color.green);

        // Debug info
        Debug.Log($"Can see player: {canSeePlayer}, Is following: {isFollowingPlayer}, Current routine: {(isFollowingPlayer ? "Following" : "Patrolling")}");

        // Check if we need to switch behaviors
        if (canSeePlayer && !isFollowingPlayer)
        {
            SwitchToFollowing();
        }
        else if (!canSeePlayer && isFollowingPlayer)
        {
            SwitchToPatrolling();
        }
    }

    private bool CanSeePlayer()
    {
        return Vector3.Distance(transform.position, player.position) <= detectionRange;
    }

    private void SwitchToFollowing()
    {
        Debug.Log("Switching to following behavior");
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }
        isFollowingPlayer = true;
        currentRoutine = StartCoroutine(FollowRoutine());
    }

    private void SwitchToPatrolling()
    {
        Debug.Log("Switching to patrolling behavior");
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }
        isFollowingPlayer = false;
        currentRoutine = StartCoroutine(PatrolRoutine());
    }

    private IEnumerator FollowRoutine()
    {
        while (isFollowingPlayer)
        {
            Debug.Log("Setting destination to player");
            agent.SetDestination(player.position);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator PatrolRoutine()
    {
        while (!isFollowingPlayer)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector3 randomPoint = centrePoint.position + Random.insideUnitSphere * patrolRange;
                if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    Debug.Log("Setting new patrol destination");
                    agent.SetDestination(hit.position);
                    yield return new WaitForSeconds(pauseDuration);
                }
            }
            yield return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}