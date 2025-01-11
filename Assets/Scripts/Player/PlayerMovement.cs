using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool IsIdle { get; private set; } = true; // because when initial event is not fired, we have wrong state
    public bool IsMoving { get; private set; }
    public bool IsFalling { get; private set; }
    public bool CanAttack
    {
        get => timeSinceLastAttack > timeBetweenAttacks;
    }

    [Header("References")]
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float jumpSpeed = 1f;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private LayerMask isGroundedLayerMask;
    [SerializeField] private float sphereJumpCheckRadius = 0.1f;
    [SerializeField] private float timeBetweenAttacks = 2f;

    private Vector2 moveDirection;
    private bool isFacingRight = true; // inital spirte facind direction
    private Rigidbody2D playerRigidbody2D;

    private float timeSinceLastAttack;

    private void Awake()
    {
        timeSinceLastAttack = float.MaxValue; // so we can attack as soon as the game starts
    }

    private void Start()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();

        inputReader.OnMovementEvent += InputReader_OnMovementEvent;
        inputReader.OnJumpEvent += InputReader_OnJumpEvent;
    }

    private void InputReader_OnJumpEvent(bool hasJumped)
    {
        if(hasJumped && IsGrounded())
        {
            HandleJump();
        }
    }

    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        HandleMovement();
        Flip();

        HandleFallingState();
    }

    private void HandleMovement()
    {
        float xMovement = moveDirection.x * movementSpeed;
        playerRigidbody2D.velocity = new Vector2(xMovement, playerRigidbody2D.velocity.y);
    }

    private void HandleJump()
    {
        playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, jumpSpeed);
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckTransform.position, sphereJumpCheckRadius, isGroundedLayerMask);
    }

    public void ResetTimeSinceLastAttack()
    {
        timeSinceLastAttack = 0f;
    }    

    private void Flip()
    {
        if(isFacingRight && moveDirection.x < 0f || !isFacingRight && moveDirection.x > 0f)
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
        IsFalling = !grounded && playerRigidbody2D.velocity.y < 0;
    }

    private void InputReader_OnMovementEvent(Vector2 moveDir)
    {
        moveDirection = moveDir;
        IsIdle = moveDir.magnitude == 0;
        IsMoving = moveDir.magnitude != 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheckTransform.position, sphereJumpCheckRadius);
    }
}
