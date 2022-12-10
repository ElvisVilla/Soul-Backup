using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Bissash;


public class Player : MonoBehaviour
{
    Rigidbody2D body;
    public SpriteRenderer Sprite { get; private set; }
    Animator anim;
    Sensor sensor;
    PlayerInputActions playerInputActions;
    public float firstTimerCooldown = 5f;
    public float secondTimerCooldown = 5f;

    public GameEvent gameStartEvent;
  
    Vector2 inputAxis = Vector2.zero;

    [Header("Ground and Wall Detection Variables")]
    [SerializeField] Transform raycastPosition;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool isWallDetected = false;
    [SerializeField] private bool canWallSlide;
    [SerializeField] private bool isWallSliding;
    private float groundRaycastDistance = 0.35f;
    private float wallRaycastDistance = 0.3f;

    [Header("Movement Values")]
    public bool canMove = true;
    [SerializeField] private float facingDirection = 1f;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashSeconds = 0.3f;
    [SerializeField] private float slidingSpeed = 0.1f;

    [Header("Jump Variables")]
    [SerializeField] private float wallJumpForce = 6;
    [SerializeField] private float doubleJumpForce = 14f;
    [SerializeField] private float jumpForce = 16f;
    [SerializeField] private float jumpBuffer = 0.2f;
    [SerializeField] private float coyoteTimeBuffer = 0.2f;
    public float jumpBufferCounter;
    public float coyoteTimeCounter;
    [SerializeField]private bool canDoubleJump = false;
    [SerializeField]private bool jumpPressed;

    // Start is called before the first frame update
    void Awake()
    {
        playerInputActions = new PlayerInputActions();
        body = GetComponent<Rigidbody2D>();
        Sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        
    }

    private void OnEnable()
    {

        playerInputActions.Enable();
        playerInputActions.Player.Attack.started += Attack_started;
        playerInputActions.Player.Dash.started += Dash_started;
        playerInputActions.Player.Dagger.started += Dagger_started;

        playerInputActions.Player.Jump.started += Jump_started;
        playerInputActions.Player.Jump.canceled += Jump_canceled;
    }

    private void Jump_started(InputAction.CallbackContext context)
    {
        gameStartEvent.RaiseEvent();
        if (isGrounded && context.started)
            canDoubleJump = false;

        jumpBufferCounter = jumpBuffer;
        jumpPressed = context.started;
        if (isWallSliding && jumpPressed)
        {
            JumpAction();
        }

        if (coyoteTimeCounter > 0 && jumpBufferCounter > 0 ||jumpPressed && canDoubleJump)
        {
            JumpAction();
            jumpBufferCounter = 0;
        }
    }

    private void Jump_canceled(InputAction.CallbackContext context)
    {
        if (context.canceled && body.velocity.y >= 0f)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y * 0.65f);
            coyoteTimeCounter = 0f;
        }
    }

    private void Dagger_started(InputAction.CallbackContext obj)
    {
            anim.SetTrigger("Dagger");
    }

    private void Dash_started(InputAction.CallbackContext obj)
    {
        StartCoroutine(Dash());
    }

    private void Attack_started(InputAction.CallbackContext obj)
    {
        anim.SetTrigger("FirstAttack");
    }

    // Update is called once per frame
    void Update()
    {

        inputAxis = playerInputActions.Player.Move.ReadValue<Vector2>();
        Animations();

        if (coyoteTimeCounter > 0 && jumpBufferCounter > 0)
        {
            JumpAction();
            jumpBufferCounter = 0;
        }

        CoyoteTimer();
        JumpBufferTimer();
    }

    private void JumpBufferTimer()
    {
        jumpBufferCounter -= Time.deltaTime;   
    }

    private void CoyoteTimer()
    {
        if (isGrounded)
            coyoteTimeCounter = coyoteTimeBuffer;
        else
            coyoteTimeCounter -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Move();
        CollisionChecks();
        WallSlide();

        body.velocity = new Vector2(body.velocity.x , Mathf.Clamp(body.velocity.y, -25f, 100f));
    }
    private void Move()
    {
        //if (wallJumping && isGrounded)
        //    canMove = true;

        if (canMove)
        {
            body.velocity = new Vector2(inputAxis.x * speed, body.velocity.y);
            FlipSprite();
        }
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(raycastPosition.position, Vector2.down,
                    groundRaycastDistance, whatIsGround);

        isWallDetected = Physics2D.Raycast(raycastPosition.position, new Vector2(facingDirection, 0),
            wallRaycastDistance, whatIsGround);
    }

    private void JumpAction()
    {
        canWallSlide = false;
        if (isWallSliding)
        {
            StartCoroutine(WallJump());
            FlipSprite();
        }
        else if (coyoteTimeCounter > 0 || canDoubleJump)
        {
            canDoubleJump = !canDoubleJump;
            var jumpStrength = canDoubleJump ? jumpForce : doubleJumpForce; 
            Jump(jumpStrength);
        }
    }

    private void WallSlide()
    {
        if (canWallSlide)
        {
            isWallSliding = true;
            body.velocity = new Vector2(body.velocity.x, body.velocity.y * slidingSpeed);
        }

        if (!isWallDetected)
        {
            isWallSliding = false;
            canWallSlide = false;
        }

        if (isWallDetected && body.velocity.y <= 0)
        {
            canWallSlide = true;
        }
    }

    private IEnumerator WallJump()
    {
        body.velocity = new Vector2(-facingDirection * 6, wallJumpForce);
        FlipSprite();
        canMove = false;
        yield return new WaitForSeconds(0.18f);
        canMove = true;
    }

    private IEnumerator Dash()
    {
        var previousSpeed = speed;
        var previousGravity = body.gravityScale;
        canMove = false;
        anim.SetTrigger("Dash");
        speed = dashSpeed;
        body.gravityScale = 0f;
        body.velocity = new Vector2(facingDirection * speed, 0);

        yield return new WaitForSeconds(0.4f);
        canMove = true;
        body.velocity = Vector2.zero;
        body.gravityScale = previousGravity;
        speed = previousSpeed;
    }

    private void Jump(float force = 16f)
    {
        anim.SetTrigger("Jump");
        body.velocity = new Vector2(body.velocity.x, force);
        canMove = true;
    }

    void FlipSprite()
    {
        //Sliding takes controll of SpriteRotation first.
        if (isWallSliding)
        {
            if (facingDirection  > 0)
            {
                Sprite.flipX = true;
            }
            else
            {
                Sprite.flipX = false;
            }
            return;
        }
        else if (inputAxis != Vector2.zero)
        {
            Sprite.flipX = inputAxis.x < 0;
            facingDirection = inputAxis.x;
        }
        else
        {
            if (facingDirection >= 0)
            {
                Sprite.flipX = false;
            }
            else
            {
                Sprite.flipX = true;
            }
        }
    }

    void Animations()
    {
        var isMoving = body.velocity.x != 0;
        anim.SetBool("IsMoving", isMoving);
        anim.SetBool("IsGround", isGrounded);
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
        playerInputActions.Player.Attack.started -= Attack_started;
        playerInputActions.Player.Dash.started -= Dash_started;
        playerInputActions.Player.Dagger.started -= Dagger_started;

        playerInputActions.Player.Jump.started -= Jump_started;
        playerInputActions.Player.Jump.canceled -= Jump_canceled;
    }

    private void OnDrawGizmos()
    {
        var lineDir = new Vector2(raycastPosition.position.x, raycastPosition.position.y -
            groundRaycastDistance);
        Gizmos.DrawLine(raycastPosition.position, lineDir);

        var wallLine = new Vector2(raycastPosition.position.x + wallRaycastDistance * facingDirection,
            raycastPosition.position.y);
        Gizmos.DrawLine(raycastPosition.position, wallLine);
    }
}