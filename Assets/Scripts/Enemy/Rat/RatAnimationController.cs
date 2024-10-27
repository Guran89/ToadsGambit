using UnityEngine;
using UnityEngine.AI;

public class RatAnimationController : MonoBehaviour
{
    private static readonly int IsIdle = Animator.StringToHash("isIdle");
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private Animator _animator;
    public NavMeshAgent _agent;

    public bool _isCurrentlyWalking;

    private void Start()
    { 
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (IsMoving())
        {
            _animator.SetBool(IsWalking, true);
            _isCurrentlyWalking = true;
        }

        if (!IsMoving())
        {
            _animator.SetBool(IsWalking, false);
            _animator.SetBool(IsIdle, true);
            _isCurrentlyWalking = false;
        }
    }

    private bool IsMoving()
    {
        return _agent.velocity.magnitude > 0.1f;
    }
}
