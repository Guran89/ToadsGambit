using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _patrolRange = 10f;
    [SerializeField] private float _pauseDuration = 2f;
    [SerializeField] private Transform _centrePoint;
    [SerializeField] private float _detectionRange = 5f;
    [SerializeField] private LayerMask _playerLayer;

    private NavMeshAgent _agent;
    private Transform _player;
    private bool _isFollowingPlayer;
    private Coroutine _currentRoutine;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        if (_player == null)
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
        }
        _currentRoutine = StartCoroutine(PatrolRoutine());
    }

    private void Update()
    {
        bool canSeePlayer = CanSeePlayer();

        // Visualize player detection
        Debug.DrawLine(transform.position, _player.position, canSeePlayer ? Color.red : Color.green);

        // Debug info
        Debug.Log($"Can see player: {canSeePlayer}, Is following: {_isFollowingPlayer}, Current routine: {(_isFollowingPlayer ? "Following" : "Patrolling")}");

        // Check if we need to switch behaviors
        if (canSeePlayer && !_isFollowingPlayer)
        {
            SwitchToFollowing();
        }
        else if (!canSeePlayer && _isFollowingPlayer)
        {
            SwitchToPatrolling();
        }
    }

    private bool CanSeePlayer()
    {
        return Vector3.Distance(transform.position, _player.position) <= _detectionRange;
    }

    private void SwitchToFollowing()
    {
        Debug.Log("Switching to following behavior");
        if (_currentRoutine != null)
        {
            StopCoroutine(_currentRoutine);
        }
        _isFollowingPlayer = true;
        _currentRoutine = StartCoroutine(FollowRoutine());
    }

    private void SwitchToPatrolling()
    {
        Debug.Log("Switching to patrolling behavior");
        if (_currentRoutine != null)
        {
            StopCoroutine(_currentRoutine);
        }
        _isFollowingPlayer = false;
        _currentRoutine = StartCoroutine(PatrolRoutine());
    }

    private IEnumerator FollowRoutine()
    {
        while (_isFollowingPlayer)
        {
            Debug.Log("Setting destination to player");
            _agent.SetDestination(_player.position);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator PatrolRoutine()
    {
        while (!_isFollowingPlayer)
        {
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                Vector3 randomPoint = _centrePoint.position + Random.insideUnitSphere * _patrolRange;
                if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    Debug.Log("Setting new patrol destination");
                    _agent.SetDestination(hit.position);
                    yield return new WaitForSeconds(_pauseDuration);
                }
            }
            yield return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _patrolRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }
}