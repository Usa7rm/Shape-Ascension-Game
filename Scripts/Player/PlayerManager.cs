using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Sprites")]
    public Sprite squareSprite;
    public Sprite circleSprite;

    private SquareMovement squareMovement;
    private CircleMovement circleMovement;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;

    private bool isCircleForm = false;
    private Animator animator;  // Reference to Animator

    // Variable to check if the player has collected the transformation item
    private bool hasCollectedItem = false;

    void Start()
    {
        // Get components from the Player GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();  // Initialize the animator

        squareMovement = GetComponent<SquareMovement>();
        circleMovement = GetComponent<CircleMovement>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();

        // Initialize to square form
        SetForm(false);

        // Ensure the animator starts with the correct form
        animator.SetBool("isCircleForm", isCircleForm);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && hasCollectedItem)  // Allow transformation only if item is collected
        {
            isCircleForm = !isCircleForm;
            TriggerTransformation();
        }
    }

    public void CollectItem()
    {
        hasCollectedItem = true;  // Set to true when the player collects the item
    }

    void TriggerTransformation()
    {
        // Trigger the transformation animation
        animator.SetTrigger("Transform");

        // After the transformation animation is done, switch the form
        StartCoroutine(SwitchFormAfterAnimation());
    }

    System.Collections.IEnumerator SwitchFormAfterAnimation()
    {
        // Wait for the animation to finish (adjust this time to match the duration of your transformation animation)
        yield return new WaitForSeconds(0.5f);  // Adjust the wait time to match the animation duration

        // Now set the form (change sprite and enable/disable movement)
        SetForm(isCircleForm);

        // Update the isCircleForm parameter in the Animator to reflect the current form
        animator.SetBool("isCircleForm", isCircleForm);
    }

    void SetForm(bool circle)
    {
        if (circle)
        {
            // Switch to circle form
            spriteRenderer.sprite = circleSprite;
            squareMovement.enabled = false;
            circleMovement.enabled = true;
            boxCollider.enabled = false;
            circleCollider.enabled = true;
        }
        else
        {
            // Switch to square form
            spriteRenderer.sprite = squareSprite;
            squareMovement.enabled = true;
            circleMovement.enabled = false;
            boxCollider.enabled = true;
            circleCollider.enabled = false;
        }
    }
}
