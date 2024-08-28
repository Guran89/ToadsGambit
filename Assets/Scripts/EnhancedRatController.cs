using UnityEngine;
using UnityEngine.AI;

public class EnhancedRatController : MonoBehaviour
{
    public float idleTime = 3f;
    public float moveRadius = 5f;
    public float animationBlendSpeed = 5f;

    private NavMeshAgent agent;
    private Animator animator;
    private float timer;
    private bool isMoving = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        timer = idleTime;
    }

    void Update()
    {
        if (isMoving)
        {
            if (agent.remainingDistance < 0.1f)
            {
                StopMoving();
            }
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                StartMoving();
            }
        }

        UpdateAnimation();
    }

    void StartMoving()
    {
        Vector3 randomPoint = Random.insideUnitSphere * moveRadius;
        randomPoint += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, moveRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            isMoving = true;
        }
    }

    void StopMoving()
    {
        isMoving = false;
        timer = idleTime;
    }

    void UpdateAnimation()
    {
        // Calculate the blend value based on velocity
        float currentSpeed = agent.velocity.magnitude / agent.speed;
        float targetBlend = isMoving ? currentSpeed : 0f;

        // Smoothly blend between idle and walk animations
        float currentBlend = animator.GetFloat("MoveSpeed");
        currentBlend = Mathf.Lerp(currentBlend, targetBlend, Time.deltaTime * animationBlendSpeed);
        animator.SetFloat("MoveSpeed", currentBlend);

        // Optionally, adjust animation speed based on actual movement speed
        animator.SetFloat("AnimationSpeed", 0.5f + currentSpeed * 0.5f);

        // Handle turning animation
        float turnAmount = Vector3.SignedAngle(transform.forward, agent.desiredVelocity, Vector3.up) / 180f;
        animator.SetFloat("TurnAmount", turnAmount);
    }
}