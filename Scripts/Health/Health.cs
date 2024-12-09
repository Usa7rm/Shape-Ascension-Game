using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth = 10f;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    private bool isInvulnerable = false;
    [SerializeField] private float invulnerabilityDuration = 1f;
    private SpriteRenderer spriteRenderer;

    [Header("Health Bar")]
    [SerializeField] private HealthBar healthBar;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (healthBar != null)
        {
            healthBar.SetMaxHealth((int)startingHealth);
            healthBar.SetHealth((int)currentHealth);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(1f);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvulnerable || dead)
            return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            PlayHurtSound();
            anim.SetTrigger("hurt");
            UpdateHealthBar();
            StartCoroutine(InvulnerabilityCoroutine());
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        if (!dead)
        {
            dead = true;
            anim.SetTrigger("die");
            PlayDeathSound();

            DisableMovementScripts();
            DisableShapeTransformation();
        }
    }

    public void OnDeathAnimationComplete()
    {
        // This function is called at the end of the death animation
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.GameOver();
        }
        else
        {
            Debug.LogWarning("UIManager not found! Ensure a UIManager is in the scene.");
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.SetHealth((int)currentHealth);
        }
    }

    private void DisableMovementScripts()
    {
        SquareMovement squareMovement = GetComponent<SquareMovement>();
        if (squareMovement != null)
            squareMovement.enabled = false;

        CircleMovement circleMovement = GetComponent<CircleMovement>();
        if (circleMovement != null)
            circleMovement.enabled = false;
    }

    private void DisableShapeTransformation()
    {
        PlayerManager playerManager = GetComponent<PlayerManager>();
        if (playerManager != null)
            playerManager.enabled = false;
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        float elapsed = 0f;
        while (elapsed < invulnerabilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }
        spriteRenderer.enabled = true;
        isInvulnerable = false;
    }

    public void AddHealth(float value)
    {
        if (dead)
            return;

        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingHealth);
        UpdateHealthBar();
    }

    private void PlayDeathSound()
    {
        if (deathSound != null)
        {
            SoundManager.instance.PlaySound(deathSound);
        }
    }

    private void PlayHurtSound()
    {
        if (hurtSound != null)
        {
            SoundManager.instance.PlaySound(hurtSound);
        }
    }
}
