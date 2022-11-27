using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D body;
    SpriteRenderer sprite;
    Animator anim;
  
    Vector2 inputAxis = Vector2.zero;

    [Header("Ground Detection Variables")]
    [SerializeField] Transform raycastPosition;
    public LayerMask whatIsGround;
    public float groundRaycastDistance = 0.1f;
    public bool isGrounded = false;
    public float wallRaycastDistance = 0.1f;
    public bool isWallDetected = false;
    private bool canWallSlide;
    private bool isWallSliding;


    [Header("Movement Values")]
    public float speed = 5.0f;
    private float facingDirection = 1f;
    private bool canDoubleJump;
    private float jumpForce = 16f;
    private float doubleJumpForce = 14f;
    public float dashSpeed = 15f;
    public float dashSeconds = 0.3f;
    public float slidingSpeed = 0.1f;
    private bool canMove = true;
    public float wallJumpForce = 6;

    [SerializeField] private float jumpBuffer = 0.2f;
    [SerializeField] private float coyoteTimeBuffer = 0.2f;
    private float jumpBufferCounter;
    private float coyoteTimeCounter;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        inputAxis.x = Input.GetAxisRaw("Horizontal");
        inputAxis.y = Input.GetAxisRaw("Vertical");
        Animations();
        InputChecks();

        if (isGrounded && !Input.GetKey(KeyCode.Space))
            canDoubleJump = false;

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTimeBuffer;         
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBuffer;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0 && jumpBufferCounter > 0|| Input.GetKeyDown(KeyCode.Space) && canDoubleJump)
        {
            JumpAction();
            jumpBufferCounter = 0f;
        }

        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0f)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }
    }
    private void FixedUpdate()
    {
        Move();
        CollisionChecks();
        WallSlide();
    }
    private void Move()
    {

        if (canMove)
        {
            body.velocity = new Vector2(inputAxis.x * speed, body.velocity.y);
        }
            FlipSprite();
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(raycastPosition.position, Vector2.down,
                    groundRaycastDistance, whatIsGround);
        anim.SetBool("IsGround", isGrounded);

        isWallDetected = Physics2D.Raycast(raycastPosition.position, Vector2.right * facingDirection,
            wallRaycastDistance, whatIsGround);
    }

    private void InputChecks()
    {

        if (isWallSliding && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)))
        {
            JumpAction();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("FirstAttack");
        }

        if (Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger("Dagger");
        }

        if (isWallDetected && body.velocity.y <= 0)
        {
            canWallSlide = true;
        }

        if (inputAxis.y < 0f)
            canWallSlide = false;
    }

    private void JumpAction()
    {

        if (isWallSliding)
        {
            canWallSlide = false;
            StartCoroutine(WallJump());
            FlipSprite();
        }
        else if (coyoteTimeCounter > 0 || canDoubleJump)
        {
            canDoubleJump = !canDoubleJump;
            canWallSlide = false;
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
    }

    private IEnumerator WallJump()
    {
        body.velocity = new Vector2(-facingDirection * wallJumpForce, jumpForce);
        canMove = false;
        yield return new WaitForSeconds(0.18f);
        canMove = true;
    }

    private IEnumerator Dash()
    {
        anim.SetTrigger("Dash");
        speed = dashSpeed;
        body.gravityScale = 0f;
        body.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.4f);
        body.velocity = Vector2.zero;
        body.gravityScale = 4;
        speed = 5f;
    }

    private void Jump(float force = 16f)
    {
        anim.SetTrigger("Jump");
        body.velocity = new Vector2(body.velocity.x, force);
        canMove = true;
    }

    void FlipSprite()
    {
        if (body.velocity.x != 0)
        {
            //Sliding takes controll of SpriteRotation.
            if (isWallSliding)
            {
                if (facingDirection == -1)
                {
                    sprite.flipX = false;
                }
                else
                {
                    sprite.flipX = true;
                }

                return;
            }

            var moving = body.velocity.x <= 0;
            sprite.flipX = moving;
            facingDirection = inputAxis.x;
        }
    }

    void Animations()
    {
        var isMoving = body.velocity.x != 0;
        anim.SetBool("IsMoving", isMoving);
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
