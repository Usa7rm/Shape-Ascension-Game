using UnityEngine;

public class SpikeHead : MonoBehaviour
{
    [Header("SpikeHead Attributes")]
    [SerializeField] private float speed = 3f;          // Movement speed
    [SerializeField] private float range = 5f;          // Detection range for the player
    [SerializeField] private float checkDelay = 0.5f;   // Delay between player checks
    [SerializeField] private LayerMask playerLayer;     // Layer to detect the player
    [SerializeField] private LayerMask groundLayer;     // Layer to detect walls and ground
    [SerializeField] private float damage = 1f;         // Damage dealt to the player

    private Vector3[] directions = new Vector3[4];      // Directions for checking the player
    private Vector3 destination;                        // Current movement direction
    private Vector3 initialPosition;                    // Initial position for resetting
    private float checkTimer;                           // Timer for player detection
    private bool attacking;                             // Whether the SpikeHead is attacking

    private Rigidbody2D rb;

    private void Start()
    {
        // Assign Rigidbody2D and ensure it's present
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is not attached to the SpikeHead GameObject.");
        }
        else
        {
            Debug.Log("Rigidbody2D assigned successfully.");
        }

        // Initialize the SpikeHead
        initialPosition = transform.position;
        Stop();
    }

    private void Update()
    {
        // Check for the player periodically if not attacking
        if (!attacking)
        {
            checkTimer += Time.deltaTime;
            if (checkTimer >= checkDelay)
            {
                CheckForPlayer();
            }
        }
    }

    private void FixedUpdate()
    {
        // Move the SpikeHead only if attacking
        if (attacking)
        {
            rb.velocity = destination * speed;
        }
    }

    private void CheckForPlayer()
    {
        CalculateDirections();

        // Check all 4 directions for the player
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i] * range, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                attacking = true;
                destination = directions[i].normalized; // Set the direction toward the player
                checkTimer = 0; // Reset the timer
                break;
            }
        }
    }

    private void CalculateDirections()
    {
        // Define the four cardinal directions
        directions[0] = Vector3.right;  // Right
        directions[1] = Vector3.left;   // Left
        directions[2] = Vector3.up;     // Up
        directions[3] = Vector3.down;   // Down
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Damage the player on collision
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Player damaged by SpikeHead!");
            }
        }
        else if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            // Stop when colliding with walls or ground
            Debug.Log("SpikeHead hit a wall or ground.");
        }

        // Stop movement on collision
        Stop();
    }

    private void Stop()
    {
        // Stop the SpikeHead and reset attacking state
        rb.velocity = Vector2.zero;
        attacking = false;
        destination = Vector3.zero;
    }

    public void ResetState()
    {
        // Reset the SpikeHead to its initial position and state
        transform.position = initialPosition;
        Stop();
        Debug.Log("SpikeHead reset to initial position.");
    }
}
