using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public float lookAheadDistance = 0.1f; // Distance to look ahead
    private Vector3 velocity = Vector3.zero;

    private Rigidbody2D targetRb;
    private bool isTransitioning = false;

    // Room boundaries
    public Vector2 minCameraPosition;
    public Vector2 maxCameraPosition;
    public bool useRoomLimits = false;

    void Start()
    {
        if (target != null)
        {
            targetRb = target.GetComponent<Rigidbody2D>();
        }
    }

    void LateUpdate()
    {
        if (isTransitioning)
            return;

        if (target != null)
        {
            // Calculate the look-ahead position based on player's velocity
            Vector3 lookAheadPos = Vector3.zero;

            if (targetRb != null)
            {
                lookAheadPos = new Vector3(targetRb.velocity.x, targetRb.velocity.y, 0) * lookAheadDistance;
            }

            Vector3 desiredPosition = target.position + offset + lookAheadPos;

            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

            if (useRoomLimits)
            {
                smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minCameraPosition.x, maxCameraPosition.x);
                smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minCameraPosition.y, maxCameraPosition.y);
            }

            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }
    }

    public IEnumerator MoveToRoom(Vector3 newPosition, float duration)
    {
        isTransitioning = true;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(newPosition.x, newPosition.y, startPosition.z);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        isTransitioning = false;

        // Reassign target and update Rigidbody2D reference
        target = GameObject.FindWithTag("Player").transform;
        targetRb = target.GetComponent<Rigidbody2D>();
    }

    public void SetRoomLimits(Vector2 minPos, Vector2 maxPos)
    {
        minCameraPosition = minPos;
        maxCameraPosition = maxPos;
        useRoomLimits = true;
    }

    public void DisableRoomLimits()
    {
        useRoomLimits = false;
    }
}

