using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour
{
    public bool IsIdle { get; private set; } = true; // because when initial event is not fired, we have wrong state
    public bool IsMoving { get; private set; }
    public bool IsFalling { get; private set; }
    public bool IsClimbing => isStandingNearLadder && Mathf.Abs(moveInput.y) > 0f;
    public bool CanAttack => timeSinceLastAttack > timeBetweenAttacks;

    [Header("References")]
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private LayerMask isGroundedLayerMask; // Ground, Obstacle

    [Header("Movement Configs")]
    [SerializeField] private float movementSpeed = 2.5f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float sphereJumpCheckRadius = 0.1f;
    [SerializeField] private float timeBetweenAttacks = 0.5f;
    [SerializeField] private float climbLadderSpeed = 40f;

    private Vector2 moveInput;
    private Collider2D playerCollider2D;
    private Rigidbody2D playerRigidbody2D;

    private bool isFacingRight = true; // inital spirte facind direction
    private float timeSinceLastAttack;
    private bool isStandingNearLadder;

    private void Awake()
    {
        timeSinceLastAttack = float.MaxValue; // so we can attack as soon as the game starts

        playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerCollider2D = GetComponent<Collider2D>();
    }

    private void Start()
    {
        inputReader.OnMovementEvent += InputReader_OnMovementEvent;
        inputReader.OnJumpEvent += InputReader_OnJumpEvent;
    }

    private void InputReader_OnJumpEvent(bool hasJumped)
    {
        if (hasJumped && IsGrounded())
        {
            HandleJump();
        }
    }

    [SerializeField] private LayerMask ladderLayerMask;
    private void Update()
    {
        isStandingNearLadder = playerCollider2D.IsTouchingLayers(ladderLayerMask);
        timeSinceLastAttack += Time.deltaTime;
        Flip();

        HandleFallingState();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleClimbing();
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckTransform.position, sphereJumpCheckRadius, isGroundedLayerMask);
    }

    public void ResetTimeSinceLastAttack()
    {
        timeSinceLastAttack = 0f;
    }

    private void HandleMovement()
    {
        float xMovement = moveInput.x * movementSpeed;
        playerRigidbody2D.velocity = new Vector2(xMovement, playerRigidbody2D.velocity.y);
    }

    private void HandleJump()
    {
        playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, jumpSpeed);
    }

    private void HandleClimbing()
    {
        if (IsClimbing)
        {
            playerRigidbody2D.gravityScale = 0f;
            playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, moveInput.y * climbLadderSpeed * Time.fixedDeltaTime);
        }
        else
        {
            playerRigidbody2D.gravityScale = 1f;
        }
    }

    private void Flip()
    {
        if (isFacingRight && moveInput.x < 0f || !isFacingRight && moveInput.x > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void HandleFallingState()
    {
        bool grounded = IsGrounded();
        IsFalling = !grounded && playerRigidbody2D.velocity.y < 0 && IsClimbing == false;
    }

    private void InputReader_OnMovementEvent(Vector2 moveDir)
    {
        moveInput = moveDir;
        IsIdle = moveDir.magnitude == 0;
        IsMoving = moveDir.magnitude != 0;
    }

    public abstract void Attack();
}
