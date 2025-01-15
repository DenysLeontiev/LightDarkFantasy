using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainBear : MonoBehaviour
{
    [Header("Enemy Behavior Configs")]
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float dwellTimeAtWayPoint = 1f;
    [SerializeField] private float timeBetweenAttacks = 3f;
    [SerializeField] private float attackRadius = 1f;

    [SerializeField] private List<Transform> waypoints;

    private readonly int isWalkingHash = Animator.StringToHash("isWalking");
    private readonly int shouldAttackHash = Animator.StringToHash("shouldAttack");
    private readonly int getHitHash = Animator.StringToHash("getHit");

    private EnemyState state;
    private Animator enemyAnimator;
    private Collider2D enemyCollider;

    private Vector2 targetPosition;
    private PlayerBase attackTarget;

    private Transform targetWaypoint;
    private Transform attackPoint;

    private float timeAtWayPoint = 0f;
    private int currentWaypointIndex = 0;
    private float timeSinceLastAttack;

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();

        attackPoint = transform.Find("AttackPoint");

        state = EnemyState.Walk;

        currentWaypointIndex = Random.Range(0, waypoints.Count - 1);
        targetWaypoint = waypoints[currentWaypointIndex];
        targetPosition = new Vector2(targetWaypoint.position.x, transform.position.y);
        timeSinceLastAttack = float.MaxValue; // so we can attack right away

        attackTarget = FindObjectOfType<PlayerBase>();
    }
    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        if(timeSinceLastAttack >= timeBetweenAttacks)
        {
            if (Vector2.Distance(transform.position, attackTarget.transform.position) < 1f)
            {
                state = EnemyState.Attack;
            }
        }

        HandleWalkingIdleAnimations();

        switch (state)
        {
            case EnemyState.Idle:
                HandleIdleState();
                break;
            case EnemyState.Walk:
                HandleWalkState();
                break;
            case EnemyState.Attack:
                HandleAttackState();
                break;
            case EnemyState.Die:
                break;
            default:
                break;
        }
    }

    private void HandleIdleState()
    {
        timeAtWayPoint += Time.deltaTime;
        if (timeAtWayPoint > dwellTimeAtWayPoint)
        {
            timeAtWayPoint = 0f;
            state = EnemyState.Walk;
            targetWaypoint = GetNextWaypoint();
        }
    }

    private void HandleWalkState()
    {
        Vector2 targetPosition = new Vector2(targetWaypoint.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, walkSpeed * Time.deltaTime);
        FaceTowards(targetPosition);
        float clampDistance = 0.01f;
        if (Vector2.Distance(transform.position, targetPosition) < clampDistance)
        {
            transform.position = targetPosition;
            state = EnemyState.Idle;
        }
    }

    private void HandleAttackState()
    {
        FaceTowards(attackTarget.transform.position);

        if (timeSinceLastAttack >= timeBetweenAttacks)
        {
            enemyAnimator.SetTrigger(shouldAttackHash);

            state = EnemyState.Idle;
            timeSinceLastAttack = 0f;
        }
    }

    private void HandleWalkingIdleAnimations()
    {
        bool isWalking = state == EnemyState.Walk;
        enemyAnimator.SetBool(isWalkingHash, isWalking);
    }

    private Transform GetNextWaypoint()
    {
        currentWaypointIndex++;
        if(currentWaypointIndex > waypoints.Count - 1)
        {
            currentWaypointIndex = 0;
        }

        return waypoints[currentWaypointIndex];
    }

    private void FaceTowards(Vector2 dir)
    {
        dir = new Vector2(dir.x, transform.position.y);
        if (transform.position.x < dir.x)
        {
            transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    // Animation Event
    public void AttackAnimationEvent()
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius);
        foreach (Collider2D collider in collider2DArray)
        {
            if (collider.TryGetComponent<Health>(out Health health) && collider != enemyCollider) // avoid attacking ourselfs
            {
                health.TakeDamage();
                return;
            }
        }
    }
}
