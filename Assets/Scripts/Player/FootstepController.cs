using UnityEngine;
using UnityEngine.Serialization;

public class FootstepController : MonoBehaviour
{
    [FormerlySerializedAs("controller")] public PlayerController _controller;
    [FormerlySerializedAs("audioSource")] public AudioSource _audioSource;
    [FormerlySerializedAs("clip")] public AudioClip[] _clip;

    [FormerlySerializedAs("walkFootstepSpeed")] public float _walkFootstepSpeed = 0.5f;
    [FormerlySerializedAs("runFootstepSpeed")] public float _runFootstepSpeed = 0.35f;
    private float _timeSinceLastFootstep;

    private void Update()
    {
        if (_controller._isMoving && !_controller._isRunning)
        {
            if (Time.time - _timeSinceLastFootstep >=_walkFootstepSpeed)
            {
                AudioClip footstepSound = _clip[Random.Range(0, _clip.Length - 2)];
                _audioSource.PlayOneShot(footstepSound);

                _timeSinceLastFootstep = Time.time;
            }
        }
        if (_controller._isRunning && _controller._isMoving)
        {
            if (Time.time - _timeSinceLastFootstep >= _runFootstepSpeed)
            {
                AudioClip footstepSound = _clip[Random.Range(2, _clip.Length)];
                _audioSource.PlayOneShot(footstepSound);

                _timeSinceLastFootstep = Time.time;
            }
        }
    }

}
