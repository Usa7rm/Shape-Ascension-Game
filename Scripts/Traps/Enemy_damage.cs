using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    [SerializeField] private float damage = 1f;           // Damage dealt by spikes
    [SerializeField] private float damageInterval = 0.5f; // Time interval between damages
    private bool isPlayerInContact = false;
    private float damageTimer = 0f;

    private void Update()
    {
        if (isPlayerInContact)
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= damageInterval)
            {
                DealDamageToPlayer();
                damageTimer = 0f; // Reset the timer
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInContact = true; // Player is in contact
            DealDamageToPlayer();    // Deal initial damage immediately
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInContact = false; // Player left the spikes
            damageTimer = 0f;          // Reset the timer
        }
    }

    private void DealDamageToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var health = player.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}
