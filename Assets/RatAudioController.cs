using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RatAudioController : MonoBehaviour
{
    [FormerlySerializedAs("audioSource")] public AudioSource _audioSource;
    [FormerlySerializedAs("ratAnimationController")] public RatAnimationController _ratAnimationController;

    [FormerlySerializedAs("footstepSpeed")] public float _footstepSpeed = 0.3f;
    private float _timeSinceLastFootstep;

    private void Update()
    {
        if (_ratAnimationController._isCurrentlyWalking)
        {
            if (Time.time - _timeSinceLastFootstep >= _footstepSpeed)
            {
                AudioClip footstepSound = _audioSource.clip;
                _audioSource.PlayOneShot(footstepSound);

                _timeSinceLastFootstep = Time.time;
            }
        }
    }
}
