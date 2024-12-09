using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }

    [Header("Background Music Clips")]
    public AudioClip defaultMusic;      // Music for default or fallback
    public AudioClip level1Music;       // Example music for "Level1" scene
    public AudioClip level2Music;       // Example music for "Level2" scene
    // Add more clips as needed

    private AudioSource audioSource;
    private AudioClip currentMusic; // Keep track of the currently playing music

    private void Awake()
    {
        // Ensure a single instance of the SoundManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Destroy duplicates
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found on the SoundManager. Please add one.");
        }

        // Subscribe to scene load event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe when destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Called whenever a new scene is loaded.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Decide which music to play based on the scene name
        AudioClip selectedMusic = SelectMusicForScene(scene.name);

        // Play if it's different from current track or if nothing is playing
        if (selectedMusic != currentMusic)
        {
            PlayBackgroundMusic(selectedMusic);
        }
    }

    /// <summary>
    /// Choose the appropriate music clip for a given scene name.
    /// </summary>
    private AudioClip SelectMusicForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "Level1":
                return level1Music;
            case "Level2":
                return level2Music;
            // Add more cases for additional levels/scenes
            default:
                return defaultMusic;
        }
    }

    /// <summary>
    /// Play a one-shot sound effect.
    /// </summary>
    public void PlaySound(AudioClip _sound)
    {
        if (_sound == null)
        {
            Debug.LogWarning("Attempted to play a null AudioClip.");
            return;
        }

        audioSource.PlayOneShot(_sound);
    }

    /// <summary>
    /// Play (or change) looping background music.
    /// </summary>
    public void PlayBackgroundMusic(AudioClip _music)
    {
        if (_music == null)
        {
            Debug.LogWarning("Attempted to play a null AudioClip as background music.");
            return;
        }

        // Stop current music if any
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        currentMusic = _music;
        audioSource.clip = _music;
        audioSource.loop = true;
        audioSource.Play();
    }

    /// <summary>
    /// Stop all sounds currently playing.
    /// </summary>
    public void StopAllSounds()
    {
        audioSource.Stop();
        currentMusic = null;
    }
}
