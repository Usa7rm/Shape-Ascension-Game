using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;      // Reduced speed for better control
    public float rollSpeed = 3f;    // Reduced rolling speed
    public float jumpForce = 6f;      // Slightly reduced jump force

    private Rigidbody2D rb;
    private bool isGrounded;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float checkRadius = 1f;    // Reduced detection radius for better precision
    public LayerMask groundLayer;

    [Header("Sound Settings")]
    public AudioClip rollingSound;    // Sound played during rolling movement
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.None; // Allow rotation

        // Ensure groundCheck is assigned
        if (groundCheck == null)
        {
            groundCheck = new GameObject("GroundCheck").transform;
            groundCheck.SetParent(transform);
            groundCheck.localPosition = new Vector3(0, -0.5f, 0);
        }

        // Set up the AudioSource for rolling sound
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true; // Loop the rolling sound
            audioSource.playOnAwake = false; // Prevent sound from playing on start
        }
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        CheckGround();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");

        // Apply torque for rolling movement
        rb.AddTorque(-moveInput * rollSpeed);

        // Play rolling sound if moving and grounded
        if (Mathf.Abs(moveInput) > 0.1f && isGrounded)
        {
            if (!audioSource.isPlaying && rollingSound != null)
            {
                audioSource.clip = rollingSound;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }
}
