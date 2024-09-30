using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RatAnimationController : MonoBehaviour
{
    Animator animator;
    public NavMeshAgent agent;

    public bool isCurrentlyWalking;

    void Start()
    { 
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        bool isIdle = animator.GetBool("isIdle");
        bool isWalking = animator.GetBool("isWalking");
    }

    void Update()
    {
        if (IsMoving())
        {
            animator.SetBool("isWalking", true);
            isCurrentlyWalking = true;
        }

        if (!IsMoving())
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdle", true);
            isCurrentlyWalking = false;
        }
    }

    bool IsMoving()
    {
        return agent.velocity.magnitude > 0.1f;
    }
}
