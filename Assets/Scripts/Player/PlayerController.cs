using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputAction _moveAction;
    [SerializeField] private InputAction _runAction;
    [SerializeField] private Animator _animator;
    [SerializeField] private Camera _mainCamera;
    //[SerializeField] private AudioSource _audioSource;

    [SerializeField] private float _movementSpeed = 3f;
    [SerializeField] private float _runModifier = 1.5f;
    [SerializeField] private float _rotationSpeed = 720f;

    private static readonly int IsMovingHash = Animator.StringToHash("IsWalking");
    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");

    public bool _isMoving;
    public bool _isRunning;

    private void Start()
    {
        _moveAction.Enable();
        _runAction.Enable();
        //_audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Vector3 movement = HandleMovement();
        HandleAnimations(movement);
    }

    private Vector3 HandleMovement()
    {
        Vector2 input = _moveAction.ReadValue<Vector2>();

        Vector3 cameraForward = _mainCamera.transform.forward;
        Vector3 cameraRight = _mainCamera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 move = (cameraRight * input.x + cameraForward * input.y).normalized;

        float currentSpeed = _movementSpeed;

        if (_runAction.ReadValue<float>() > 0)
        {
            currentSpeed *= _runModifier;
        }

        if (move != Vector3.zero)
        {
            transform.position += currentSpeed * Time.deltaTime * move;
            HandleRotation(move);
        }

        return move;
    }

    private void HandleRotation(Vector3 movement)
    {
        Quaternion targetRotation = Quaternion.LookRotation(movement);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void HandleAnimations(Vector3 movement)
    {
        _isMoving = movement.magnitude > 0;
        bool isRunKeyPressed = _runAction.ReadValue<float>() > 0;

        _isRunning = _isMoving && isRunKeyPressed;

        _animator.SetBool(IsMovingHash, _isMoving);
        _animator.SetBool(IsRunningHash, _isRunning);
    }
}
