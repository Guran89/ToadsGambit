using UnityEngine;

public class FlyAnimationStateController : MonoBehaviour
{
    private static readonly int IsFlying = Animator.StringToHash("isFlying");

    private Animator _animator;
    // Start is called before the first frame update
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        _animator.SetBool(IsFlying, true);
    }
}
