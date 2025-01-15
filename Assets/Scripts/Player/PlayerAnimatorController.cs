using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    private Animator playerAnimator;
    private Knight playerMovement;
    private Health playerHealth;

    private readonly int isRunningHash = Animator.StringToHash("isRunning");
    private readonly int isFallingHash = Animator.StringToHash("isFalling");
    private readonly int isClimbingHash = Animator.StringToHash("isClimbing");
    private readonly int isJumpingHash = Animator.StringToHash("shouldJump");
    private readonly int isAttackingHash = Animator.StringToHash("shouldAttack");
    private readonly int isRunAttackingHash = Animator.StringToHash("shouldRunAttack");
    private readonly int getHitHash = Animator.StringToHash("getHit");
    private readonly int dieHash = Animator.StringToHash("die");

    private void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<Knight>();
        playerHealth = GetComponent<Health>();

        inputReader.OnMovementEvent += InputReader_OnMovementEvent;
        inputReader.OnJumpEvent += InputReader_OnJumpEvent;
        inputReader.OnAttackEvent += InputReader_OnAttackEvent;

        playerHealth.OnDamageTaken += PlayerHealth_OnDamageTaken;
        playerHealth.OnDied += PlayerHealth_OnDied;
    }

    private void OnDisable()
    {
        inputReader.OnMovementEvent -= InputReader_OnMovementEvent;
        inputReader.OnJumpEvent -= InputReader_OnJumpEvent;
        inputReader.OnAttackEvent -= InputReader_OnAttackEvent;

        playerHealth.OnDamageTaken -= PlayerHealth_OnDamageTaken;
        playerHealth.OnDied -= PlayerHealth_OnDied;
    }


    private void Update()
    {
        if (playerHealth.IsDead)
            return;

        playerAnimator.SetBool(isFallingHash, playerMovement.IsFalling);
        playerAnimator.SetBool(isClimbingHash, playerMovement.IsClimbing);
    }

    private void InputReader_OnMovementEvent(Vector2 moveDir)
    {
        bool isMoving = moveDir.x != 0;
        playerAnimator.SetBool(isRunningHash, isMoving);
    }

    private void InputReader_OnJumpEvent(bool isJumping)
    {
        if(isJumping && playerMovement.IsClimbing == false && playerMovement.IsGrounded() == true)
        {
            playerAnimator.SetTrigger(isJumpingHash);
        }
    }

    private void InputReader_OnAttackEvent(bool isAttacking)
    {
        if (isAttacking == false)
            return;

        if (playerMovement.CanAttack == false)
            return;

        bool canAttackWhileIdle = playerMovement.IsIdle && playerMovement.IsFalling == false;
        bool canAttackWhileMovingInAir = playerMovement.IsMoving && playerMovement.IsGrounded() == false && playerMovement.IsFalling == false;
        bool canAttackWhileRunning = playerMovement.IsMoving && playerMovement.IsGrounded();

        if(canAttackWhileIdle || canAttackWhileMovingInAir)
        {
            playerAnimator.SetTrigger(isAttackingHash);
            playerMovement.ResetTimeSinceLastAttack();
        }
        else if(canAttackWhileRunning)
        {
            playerAnimator.SetTrigger(isRunAttackingHash);
            playerMovement.ResetTimeSinceLastAttack();
        }
    }

    private void PlayerHealth_OnDamageTaken(object sender, System.EventArgs e)
    {
        playerAnimator.SetTrigger(getHitHash);
    }

    private void PlayerHealth_OnDied(object sender, System.EventArgs e)
    {
        playerAnimator.SetTrigger(dieHash);
    }
}
