using System.Collections;
using UnityEngine;

public class Firetrap : MonoBehaviour
{
    [Header("Trap Settings")]
    [SerializeField] private float activationDelay = 1f;   // Delay before the trap activates
    [SerializeField] private float activeDuration = 2f;   // Time the trap remains active
    [SerializeField] private float damage = 1f;           // Damage dealt to the player
    [SerializeField] private Color triggerColor = Color.red; // Color when triggered
    [SerializeField] private Color defaultColor = Color.white; // Default color

    [Header("Sound Settings")]
    [SerializeField] private AudioClip activationSound;   // Sound when the trap activates
    [SerializeField] private AudioClip damageSound;       // Sound when the trap deals damage
    private AudioSource audioSource;

    private SpriteRenderer rend;
    private Animator anim;
    private bool isActivated = false;  // Is the trap currently active?
    private bool isTriggered = false; // Has the trap been triggered?

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false; // Prevent sound from playing automatically
        }
    }

    private void Update()
    {
        // Update animator with the activation state
        anim.SetBool("activated", isActivated);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isActivated && !isTriggered)
            {
                StartCoroutine(ActivateTrap());
            }
            else if (isActivated)
            {
                collision.GetComponent<Health>().TakeDamage(damage);

                // Play damage sound
                PlaySound(damageSound);
            }
        }
    }

    private IEnumerator ActivateTrap()
    {
        // Set trap to triggered and change its color
        isTriggered = true;
        rend.color = triggerColor;

        // Play activation sound
        PlaySound(activationSound);

        // Wait for the activation delay
        yield return new WaitForSeconds(activationDelay);

        // Activate the trap
        rend.color = defaultColor;
        isActivated = true;

        // Keep the trap active for the specified duration
        yield return new WaitForSeconds(activeDuration);

        // Deactivate and reset the trap
        isActivated = false;
        isTriggered = false;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
