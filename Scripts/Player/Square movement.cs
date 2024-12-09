using UnityEngine;

public class SquareMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float smashForce = 20f;
    public float wallJumpForce = 15f;
    public float wallSlideSpeed = 2f;
    public float wallGravityScale = 0.5f;
    public float wallJumpTime = 0.2f;  // Wall jump cooldown time
    public float smashCooldown = 0.5f;  // Smash cooldown time in seconds

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isSmashing;
    private bool isTouchingWall;
    private bool isWallOnRight;
    private bool isWallSliding;
    private bool wallJumping;
    private float wallJumpTimer;
    private float smashTimer;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float checkRadius = 0.5f;
    public LayerMask groundLayer;

    [Header("Ability Settings")]
    public LayerMask obstacleLayer;
    public Transform smashPoint;
    public float smashRadius = 0.5f;

    [Header("Wall Detection")]
    public float wallCheckDistance = 0.5f;
    public LayerMask wallLayer;

    [Header("Sound Settings")]
    public AudioClip jumpSound;  // Sound for jumping
    public AudioClip smashSound; // Sound for smashing
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        HorizontalMove();
        CheckGround();
        CheckWall();
        HandleJump();
        HandleWallJump();
        HandleWallSlide();
        HandleSmash();
    }

    void HorizontalMove()
    {
        if (wallJumping)
        {
            // Do not modify horizontal velocity during wall jump
            return;
        }

        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void CheckGround()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        if (isGrounded)
        {
            if (isSmashing)
            {
                Collider2D[] hitObjects = Physics2D.OverlapCircleAll(smashPoint.position, smashRadius, obstacleLayer);
                foreach (Collider2D hit in hitObjects)
                {
                    // Destroy or interact with the obstacle/enemy
                    Destroy(hit.gameObject);
                }
                isSmashing = false;
            }

            // Reset other states if necessary
            wallJumping = false;
            wallJumpTimer = 0;
        }
    }

    void CheckWall()
    {
        // Check if the player is touching a wall on either side
        RaycastHit2D wallHitRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayer);
        RaycastHit2D wallHitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, wallLayer);
        isTouchingWall = wallHitRight.collider != null || wallHitLeft.collider != null;

        // Determine which side the wall is on
        if (wallHitRight.collider != null)
        {
            isWallOnRight = true;
        }
        else if (wallHitLeft.collider != null)
        {
            isWallOnRight = false;
        }
        else
        {
            isWallOnRight = false;
            isTouchingWall = false;
        }
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            // Play jump sound
            PlaySound(jumpSound);
        }
    }

    void HandleWallSlide()
    {
        if (!wallJumping && isTouchingWall && !isGrounded && rb.velocity.y < 0 &&
            ((isWallOnRight && Input.GetAxisRaw("Horizontal") > 0) || (!isWallOnRight && Input.GetAxisRaw("Horizontal") < 0)))
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            rb.gravityScale = wallGravityScale; // Reduce gravity while sliding to allow sticking
        }
        else
        {
            isWallSliding = false;

            // Only reset gravity scale if not wall jumping
            if (!wallJumping)
            {
                rb.gravityScale = 3f; // Restore normal gravity
            }
        }
    }

    void HandleWallJump()
    {
        if (isWallSliding && Input.GetButtonDown("Jump"))
        {
            wallJumping = true;
            wallJumpTimer = wallJumpTime;

            // Determine wall jump direction
            float jumpDirection = isWallOnRight ? -1 : 1;

            rb.AddForce(new Vector2(jumpDirection * wallJumpForce, wallJumpForce), ForceMode2D.Impulse);
            rb.gravityScale = 3f; // Restore normal gravity after jumping off the wall

            // Play jump sound
            PlaySound(jumpSound);
        }

        // Handle wall jump timing
        if (wallJumping)
        {
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer <= 0)
            {
                wallJumping = false;
            }
        }
    }

    void HandleSmash()
    {
        if (smashTimer > 0)
        {
            smashTimer -= Time.deltaTime;
        }

        if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) && !isGrounded && smashTimer <= 0)
        {
            // Perform smash by adding a downward force
            rb.velocity = new Vector2(rb.velocity.x, -smashForce);
            isSmashing = true;
            smashTimer = smashCooldown; // Set cooldown timer

            // Play the smash sound
            PlaySound(smashSound);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    void LateUpdate()
    {
        // Ensure groundCheck moves with the player
        groundCheck.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
    }

    void OnDrawGizmosSelected()
    {
        if (smashPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(smashPoint.position, smashRadius);
        }

        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * wallCheckDistance); // Right check
        Gizmos.DrawLine(transform.position, transform.position - Vector3.right * wallCheckDistance); // Left check
    }
}
