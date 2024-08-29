using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAnimationController : MonoBehaviour
{
    Animator animator;
    bool isMoving = true;
    // Start is called before the first frame update
    void Start()
    {
        
        animator = GetComponent<Animator>();
        bool isIdle = animator.GetBool("isIdle");
        bool isWalking = animator.GetBool("isWalking");
        animator.SetBool("isWalking", true);
    }

    // Update is called once per frame
    void Update()
    {
        /*if(isMoving)
        {
            animator.SetBool("isWalking", true);
        }

        if (!isMoving)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdle", true);
        }*/
    }
}
