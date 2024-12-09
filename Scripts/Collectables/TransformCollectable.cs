using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Header("Collectable Settings")]
    public string itemName = "ShapeTransformItem";  // You can name the item as you like
    private bool isCollected = false;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip collectSound; // Sound played on collection

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            // Play the collection sound
            PlayCollectSound();

            // Give the player the ability to transform
            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.CollectItem(); // Enable transformation in the PlayerManager
            }

            // Destroy the collectable item after a slight delay to allow sound to play
            Destroy(gameObject, collectSound != null ? collectSound.length : 0f);
        }
    }

    private void PlayCollectSound()
    {
        if (collectSound != null)
        {
            // Create a temporary GameObject to play the sound
            GameObject tempAudioSource = new GameObject("CollectSound");
            AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();
            audioSource.clip = collectSound;
            audioSource.playOnAwake = false;
            audioSource.Play();

            // Destroy the temporary GameObject after the sound has finished playing
            Destroy(tempAudioSource, collectSound.length);
        }
        else
        {
            Debug.LogWarning("No collect sound assigned to the collectable.");
        }
    }
}
