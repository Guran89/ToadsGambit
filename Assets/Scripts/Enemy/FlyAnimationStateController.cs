using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAnimationStateController : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        bool isFlying = animator.GetBool("isFlying");
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isFlying", true);
    }
}
