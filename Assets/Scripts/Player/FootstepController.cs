using Unity.VisualScripting;
using UnityEngine;
public class FootstepController : MonoBehaviour
{
    public PlayerController controller;
    public AudioSource audioSource;
    public AudioClip[] clip;

    public float walkFootstepSpeed = 0.5f;
    public float runFootstepSpeed = 0.35f;
    private float timeSinceLastFootstep;

    private void Update()
    {
        if (controller.isMoving && !controller.isRunning)
        {
            if (Time.time - timeSinceLastFootstep >=walkFootstepSpeed)
            {
                AudioClip footstepSound = clip[Random.Range(0, clip.Length)];
                audioSource.PlayOneShot(footstepSound);
                Debug.Log(footstepSound);

                timeSinceLastFootstep = Time.time;
            }
        }
        if (controller.isRunning && controller.isMoving)
        {
            if (Time.time - timeSinceLastFootstep >= runFootstepSpeed)
            {
                AudioClip footstepSound = audioSource.clip;
                audioSource.PlayOneShot(footstepSound);

                timeSinceLastFootstep = Time.time;
            }
        }
    }

}
