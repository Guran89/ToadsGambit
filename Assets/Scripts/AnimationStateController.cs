using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");
        bool forwardPressed = Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d");
        
        if(Input.GetKey(KeyCode.LeftShift)){
            animator.SetBool("isRunning", true);
        }
        if(isRunning && !Input.GetKey(KeyCode.LeftShift)){
            animator.SetBool("isRunning", false);
        }

        if(!isWalking && forwardPressed){
            animator.SetBool("isWalking", true);
        }
        if(isWalking && !forwardPressed){
            animator.SetBool("isWalking", false);
        }
    }
}
