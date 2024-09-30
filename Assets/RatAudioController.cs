using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public RatAnimationController ratAnimationController;

    public float footstepSpeed = 0.3f;
    private float timeSinceLastFootstep;

    private void Update()
    {
        if (ratAnimationController.isCurrentlyWalking)
        {
            if (Time.time - timeSinceLastFootstep >= footstepSpeed)
            {
                AudioClip footstepSound = audioSource.clip;
                audioSource.PlayOneShot(footstepSound);

                timeSinceLastFootstep = Time.time;
            }
        }
    }
}
