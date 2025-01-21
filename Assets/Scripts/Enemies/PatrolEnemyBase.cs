using System.Collections.Generic;
using UnityEngine;

public abstract class PatrolEnemyBase : MonoBehaviour
{
    [Header("Enemy Behavior Configs")]
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float dwellTimeAtWayPoint = 1.5f;
    [SerializeField] private float timeBetweenAttacks = 2f;
    [SerializeField] private float attackRadius = 1f;

    [SerializeField] private List<Transform> waypoints;

    private readonly int isWalkingHash = Animator.StringToHash("isWalking");
    private readonly int shouldAttackHash = Animator.StringToHash("shouldAttack");
    private readonly int getHitHash = Animator.StringToHash("getHit");
    private readonly int dieHash = Animator.StringToHash("die");

    private EnemyState state;
    private Animator enemyAnimator;
    private Health enemyHealth;

    private Collider2D enemyCollider;

    private Vector2 targetPosition;
    private PlayerBase attackTarget;

    private Transform targetWaypoint;
    private Transform attackPoint;

    private float timeAtWayPoint = 0f;
    private int currentWaypointIndex = 0;
    private float timeSinceLastAttack;

    private bool hasDiedAnimationPlayed = false;

    private void Awake()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
        enemyHealth = GetComponent<Health>();
    }

    private void Start()
    {
        attackTarget = FindObjectOfType<PlayerBase>();
        attackPoint = transform.Find("AttackPoint");

        state = EnemyState.Idle;

        currentWaypointIndex = 0;

        if (waypoints.Count != 0)
        {
            targetWaypoint = waypoints[currentWaypointIndex];
            targetPosition = new Vector2(targetWaypoint.position.x, transform.position.y);
        }
        timeSinceLastAttack = float.MaxValue; // so we can attack right away

        enemyHealth.OnDamageTaken += EnemyHealth_OnDamageTaken;
        enemyHealth.OnDied += EnemyHealth_OnDied;
    }

    private void OnDisable()
    {
        enemyHealth.OnDamageTaken -= EnemyHealth_OnDamageTaken;
        enemyHealth.OnDied -= EnemyHealth_OnDied;
    }

    private void Update()
    {
        // TODO: Refactor attackTarget init in PatrolEnemyBase
        // Not the most efficient way, can at least add timer in order not to check dozens of times per prefame
        if (attackTarget == null)
        {
            attackTarget = FindObjectOfType<PlayerBase>();
        }

        if (hasDiedAnimationPlayed)
        {
            enemyAnimator.SetTrigger(dieHash);
            hasDiedAnimationPlayed = false;
        }

        if (enemyHealth.IsDead)
        {
            return;
        }

        timeSinceLastAttack += Time.deltaTime;

        if (timeSinceLastAttack >= timeBetweenAttacks)
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
            case EnemyState.Hit:
                enemyAnimator.SetTrigger(getHitHash);
                state = EnemyState.Idle;
                break;
        }
    }

    private void HandleIdleState()
    {
        timeAtWayPoint += Time.deltaTime;
        if (timeAtWayPoint > dwellTimeAtWayPoint && targetWaypoint != null)
        {
            timeAtWayPoint = 0f;
            state = EnemyState.Walk;
            targetWaypoint = GetNextWaypoint();
        }
    }

    private void HandleWalkState()
    {
        if (targetWaypoint == null)
        {
            return;
        }

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
        if (currentWaypointIndex > waypoints.Count - 1)
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

    private void EnemyHealth_OnDamageTaken(object sender, System.EventArgs e)
    {
        state = EnemyState.Hit;
    }
    private void EnemyHealth_OnDied(object sender, System.EventArgs e)
    {
        state = EnemyState.Die;
        hasDiedAnimationPlayed = true;
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
