using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;       // Array of enemies in the room
    [SerializeField] private GameObject roomPromptText;  // HUD text to display for the room
    private Vector3[] initialPosition;                  // Stores the initial positions of the enemies
    [SerializeField] private bool isStartingRoom = false; // Set to true if this is the starting room
    private bool promptDisplayed = true;                // Track whether the prompt is displayed

    private void Awake()
    {
        // Save the initial positions of all enemies
        initialPosition = new Vector3[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
                initialPosition[i] = enemies[i].transform.position;
        }

        // If this is the starting room, display the text
        if (roomPromptText != null)
        {
            roomPromptText.SetActive(isStartingRoom);
            promptDisplayed = isStartingRoom; // Ensure the prompt is tracked correctly
        }
    }

    public void ActivateRoom(bool _status)
    {
        // Activate or deactivate enemies in the room
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].SetActive(_status); // Turn enemies on or off
                if (_status)
                {
                    enemies[i].transform.position = initialPosition[i]; // Reset position when activating
                    ResetEnemyState(enemies[i]); // Reset specific enemy states
                }
            }
        }

        // Manage the room prompt text
        if (roomPromptText != null)
        {
            // Show the prompt only if transitioning into the room for the first time
            if (_status && promptDisplayed)
            {
                roomPromptText.SetActive(true);
            }
            else
            {
                roomPromptText.SetActive(false);
                promptDisplayed = false; // Ensure the prompt does not reappear
            }
        }
    }

    private void ResetEnemyState(GameObject enemy)
    {
        // Check for SpikeHead component and reset its state
        SpikeHead spikeHead = enemy.GetComponent<SpikeHead>();
        if (spikeHead != null)
        {
            spikeHead.ResetState();
        }
    }
}
