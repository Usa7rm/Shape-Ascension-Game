using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;      // Speed of the projectile
    [SerializeField] private float resetTime = 5f;  // Time before the projectile deactivates
    [SerializeField] private float damage = 1f;     // Damage dealt by the projectile
    private float lifetime;                         // Tracks the projectile's lifetime

    public void ActivateProjectile()
    {
        // Reset lifetime and activate the projectile
        lifetime = 0;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        // Move the projectile forward
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        // Increment lifetime and deactivate if it exceeds reset time
        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
        {
            DeactivateProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Deal damage to the player
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        // Deactivate the projectile after collision
        DeactivateProjectile();
    }

    private void DeactivateProjectile()
    {
        // Disable the projectile
        gameObject.SetActive(false);
    }
}
