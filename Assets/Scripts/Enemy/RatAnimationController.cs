using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RatAnimationController : MonoBehaviour
{
    Animator animator;
    public NavMeshAgent agent;
    bool isMoving = true;


    // Start is called before the first frame update
    void Start()
    { 
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        bool isIdle = animator.GetBool("isIdle");
        bool isWalking = animator.GetBool("isWalking");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMoving())
        {
            animator.SetBool("isWalking", true);
        }

        if (!IsMoving())
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdle", true);
        }
    }

    bool IsMoving()
    {
        return agent.velocity.magnitude > 0.1f;
    }
}
