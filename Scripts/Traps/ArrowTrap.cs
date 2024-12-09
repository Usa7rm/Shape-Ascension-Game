using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [Header("Arrow Trap Settings")]
    [SerializeField] private float attackCooldown = 2f; // Time between arrow attacks
    [SerializeField] private Transform firePoint;       // Position where arrows are fired from
    [SerializeField] private GameObject[] arrows;       // Pool of arrows to reuse

    private float cooldownTimer = 0f;                   // Timer to track cooldowns

    private void Update()
    {
        // Increment cooldown timer
        cooldownTimer += Time.deltaTime;

        // If the cooldown timer reaches the threshold, attack
        if (cooldownTimer >= attackCooldown)
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Reset the cooldown timer
        cooldownTimer = 0f;

        // Find an inactive arrow in the pool and fire it
        int arrowIndex = FindArrow();
        if (arrowIndex != -1) // Ensure an arrow is available
        {
            // Set the arrow's position to the fire point
            arrows[arrowIndex].transform.position = firePoint.position;

            // Activate the projectile
            arrows[arrowIndex].GetComponent<EnemyProjectile>().ActivateProjectile();
        }
        else
        {
            Debug.LogWarning("No available arrows in the pool!");
        }
    }

    private int FindArrow()
    {
        // Search for an inactive arrow in the pool
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
                return i;
        }

        // Return -1 if no arrow is available
        return -1;
    }
}
