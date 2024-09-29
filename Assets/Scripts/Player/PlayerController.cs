using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction moveAction;
    public InputAction runAction;
    public Animator animator;
    public Camera mainCamera;

    public float movementSpeed = 3f;
    public float runModifier = 1.5f;
    public float rotationSpeed = 720f;

    private static readonly int IsMovingHash = Animator.StringToHash("IsWalking");
    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");


    void Start()
    {
        moveAction.Enable();
        runAction.Enable();
    }

    void Update()
    {
        Vector3 movement = HandleMovement();
        HandleAnimations(movement);
    }

    Vector3 HandleMovement()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 move = (cameraRight * input.x + cameraForward * input.y).normalized;

        float currentSpeed = movementSpeed;

        if (runAction.ReadValue<float>() > 0)
        {
            currentSpeed *= runModifier;
        }

        if (move != Vector3.zero)
        {
            transform.position += currentSpeed * Time.deltaTime * move;
            HandleRotation(move);
        }

        return move;
    }

    void HandleRotation(Vector3 movement)
    {
        Quaternion targetRotation = Quaternion.LookRotation(movement);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void HandleAnimations(Vector3 movement)
    {
        bool isMoving = movement.magnitude > 0;
        bool isRunKeyPressed = runAction.ReadValue<float>() > 0;

        bool isRunning = isMoving && isRunKeyPressed;

        animator.SetBool(IsMovingHash, isMoving);
        animator.SetBool(IsRunningHash, isRunning);

    }
}
