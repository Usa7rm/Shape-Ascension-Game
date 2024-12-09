using UnityEngine;

public class Enemy_Sideway : MonoBehaviour
{
    [SerializeField] private float movementDistance = 3f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float damage = 1f;
    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;

    private void Awake()
    {
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
    }

    private void Update()
    {
        // Movement logic
        if (movingLeft)
        {
            if (transform.position.x > leftEdge)
            {
                transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            }
            else
            {
                movingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightEdge)
            {
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            }
            else
            {
                movingLeft = true;
            }
        }

        // Rotation (Optional)
        transform.Rotate(0f, 0f, 360f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
