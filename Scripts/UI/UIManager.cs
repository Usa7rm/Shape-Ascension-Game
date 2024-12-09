using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject HealthBar;       // Reference to the health bar UI
    [SerializeField] private GameObject deathScreen;     // Reference to the death screen UI

    [Header("Audio Settings")]
    [SerializeField] private AudioClip gameOverSound;    // Sound effect for game over

    private void Awake()
    {
        // Ensure only the Health Bar is active at the start
        HealthBar.SetActive(true);
        deathScreen.SetActive(false);
    }

    public void GameOver()
    {
        // Deactivate other UI elements
        HealthBar.SetActive(false);

        // Activate the Death Screen
        deathScreen.SetActive(true);
        PlaySound(gameOverSound);
    }

    public void Restart()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        // Exit the application
        Application.Quit();

#if UNITY_EDITOR
        // Exit Play mode in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound(clip);
        }
    }
}
