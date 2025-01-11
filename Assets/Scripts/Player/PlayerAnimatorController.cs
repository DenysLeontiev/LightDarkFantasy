using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    private Animator playerAnimator;
    private PlayerMovement playerMovement;

    private readonly int isRunningHash = Animator.StringToHash("isRunning");
    private readonly int isFallingHash = Animator.StringToHash("isFalling");
    private readonly int isJumpingHash = Animator.StringToHash("shouldJump");
    private readonly int isAttackingHash = Animator.StringToHash("shouldAttack");
    private readonly int isRunAttackingHash = Animator.StringToHash("shouldRunAttack");

    private void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        inputReader.OnMovementEvent += InputReader_OnMovementEvent;
        inputReader.OnJumpEvent += InputReader_OnJumpEvent;
        inputReader.OnAttackEvent += InputReader_OnAttackEvent;
    }


    private void Update()
    {
        playerAnimator.SetBool(isFallingHash, playerMovement.IsFalling);
    }

    private void InputReader_OnMovementEvent(Vector2 moveDir)
    {
        bool isMoving = moveDir.x != 0;
        playerAnimator.SetBool(isRunningHash, isMoving);
    }

    private void InputReader_OnJumpEvent(bool isJumping)
    {
        if(isJumping)
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
}
