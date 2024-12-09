using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [Header("Arrow Navigation")]
    [SerializeField] private RectTransform[] buttons; // Array of buttons for Restart, Main Menu, Quit
    [SerializeField] private AudioClip changeSound;   // Sound for changing options
    [SerializeField] private AudioClip interactSound; // Sound for selecting an option
    private RectTransform arrow;                     // The arrow's RectTransform
    private int currentPosition;                     // Current index of the selected option

    private void Awake()
    {
        // Cache the arrow's RectTransform
        arrow = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        // Initialize arrow position to the first option
        currentPosition = 0;
        ChangePosition(0);
    }

    private void Update()
    {
        // Navigate up and down the menu
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            ChangePosition(-1); // Move up
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            ChangePosition(1); // Move down
        }

        // Interact with the selected option
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void ChangePosition(int _change)
    {
        // Update the current position index
        currentPosition += _change;

        // Wrap around if the position exceeds the bounds
        if (currentPosition < 0)
            currentPosition = buttons.Length - 1;
        else if (currentPosition > buttons.Length - 1)
            currentPosition = 0;

        // Play the navigation sound if the position changes
        if (_change != 0 && SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound(changeSound);
        }

        // Update the arrow's position
        AssignPosition();
    }

    private void AssignPosition()
    {
        // Set the arrow's position to align with the selected button
        arrow.position = new Vector3(arrow.position.x, buttons[currentPosition].position.y);
    }

    private void Interact()
    {
        // Play the interaction sound
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound(interactSound);
        }

        // Invoke the selected button's action
        buttons[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}
