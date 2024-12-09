using UnityEngine;
using System.Collections;

public class DoorTransition : MonoBehaviour
{
    [Header("Camera Transition Settings")]
    [SerializeField] private Transform newCameraPosition;  // Target position for the camera in the new room
    [SerializeField] private float transitionTime = 1f;    // Duration of the camera transition

    [Header("Player Transition Settings")]
    [SerializeField] private Transform newPlayerPosition;  // Target position for the player in the new room

    [Header("Room Management")]
    [SerializeField] private Transform previousRoom;       // Reference to the current room
    [SerializeField] private Transform nextRoom;           // Reference to the next room

    private CameraFollow cameraFollow;                    // Reference to the CameraFollow script
    private bool playerIsTransitioning = false;           // To prevent multiple triggers

    private void Awake()
    {
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !playerIsTransitioning)
        {
            playerIsTransitioning = true;

            // Determine the direction of the player to decide the room transition
            if (collision.transform.position.x < transform.position.x)
            {
                StartCoroutine(TransitionToRoom(nextRoom, previousRoom, collision));
            }
            else
            {
                StartCoroutine(TransitionToRoom(previousRoom, nextRoom, collision));
            }
        }
    }

    private IEnumerator TransitionToRoom(Transform activateRoom, Transform deactivateRoom, Collider2D player)
    {
        // Deactivate current room
        deactivateRoom.GetComponent<Room>().ActivateRoom(false);

        // Camera transition
        if (newCameraPosition != null)
        {
            yield return StartCoroutine(cameraFollow.MoveToRoom(newCameraPosition.position, transitionTime));
        }

        // Activate the new room
        activateRoom.GetComponent<Room>().ActivateRoom(true);

        // Move the player to the new position
        if (newPlayerPosition != null)
        {
            player.transform.position = newPlayerPosition.position;
        }

        yield return new WaitForSeconds(0.1f);
        playerIsTransitioning = false;
    }
}
