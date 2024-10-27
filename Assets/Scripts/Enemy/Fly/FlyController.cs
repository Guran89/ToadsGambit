using UnityEngine;
using UnityEngine.AI;

public class FlyController : MonoBehaviour
{
    private static readonly int IsFlying = Animator.StringToHash("isFlying");
    [SerializeField] private float _wanderRadius = 10f;
    [SerializeField] private float _wanderTimer = 5f;
    [SerializeField] private float _playerDetectionRadius = 5f;
    [SerializeField] private float _fleeDistance = 15f;
    [SerializeField] private float _fleeSpeed = 8f;
    [SerializeField] private float _normalSpeed = 3f;

    private NavMeshAgent _agent;
    private Animator _animator;
    private float _timer;
    private Transform _player;

    private enum FlyState
    {
        Idle,
        Flying,
        Fleeing
    }

    private FlyState _currentState = FlyState.Idle;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _timer = _wanderTimer;
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        // Enable flying
        _agent.baseOffset = 4.5f; // Adjust this value to set the fly's height
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if (distanceToPlayer <= _playerDetectionRadius)
        {
            FleeFromPlayer();
        }
        else
        {
            Wander();
        }

        UpdateAnimation();
    }

    private void Wander()
    {
        _timer += Time.deltaTime;

        if (_timer >= _wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, _wanderRadius, -1);
            _agent.speed = _normalSpeed;
            _agent.SetDestination(newPos);
            _timer = 0;
            _currentState = FlyState.Flying;
        }

        if (_agent.remainingDistance < 0.1f)
        {
            _currentState = FlyState.Idle;
        }
    }

    private void FleeFromPlayer()
    {
        Vector3 fleeDirection = transform.position - _player.position;
        Vector3 newPos = transform.position + fleeDirection.normalized * _fleeDistance;

        if (!NavMesh.SamplePosition(newPos, out var hit, _fleeDistance, NavMesh.AllAreas)) return;
        _agent.speed = _fleeSpeed;
        _agent.SetDestination(hit.position);
        _currentState = FlyState.Fleeing;
    }

    private static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMesh.SamplePosition(randDirection, out var navHit, dist, layermask);

        return navHit.position;
    }

    private void UpdateAnimation()
    {
        switch (_currentState)
        {
            case FlyState.Idle:
                _animator.SetBool(IsFlying, false);
                break;
            case FlyState.Flying:
            case FlyState.Fleeing:
                _animator.SetBool(IsFlying, true);
                break;
        }
    }
}