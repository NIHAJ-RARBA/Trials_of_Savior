using UnityEngine;
using UnityEngine.SceneManagement;

public class IsometricMovement : MonoBehaviour
{
    public float speed = 5f; // Movement speed of the character
    public float jumpDuration = 0.5f; // Time it takes to complete the jump cycle
    public float jumpHeight = 1.5f; // Visual jump height
    public Transform spriteTransform; // Reference to the transform of the character sprite

    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isJumping = false;
    private float jumpTimer = 0f;
    private float baseYPosition = 0f; // Track the base Y position when the jump starts
    private float yVelocity = 0f; // Track the vertical velocity
    private bool hasReachedPeak = false;

    public AudioSource soundPlayer;
    public AudioClip jumpSound;
    public AudioClip walkSound;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.SetBool("postPeak", false);
    }

    private void Update()
    {
        HandleMovementInput();

        // Check if jump button (space) is pressed and character is not jumping
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            StartJump();
            soundPlayer.clip = jumpSound;
            soundPlayer.Play();
        }

        // Handle the visual jump sequence
        if (isJumping)
        {
            HandleJump();
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        
    }

    private void HandleMovementInput()
    {
        // Get input for movement (WASD or arrow keys)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Calculate movement vector
        movement = new Vector2(horizontal, vertical).normalized;

        // Update the animator parameter "speed" based on the magnitude of the movement
        animator.SetFloat("speed", movement.magnitude * speed);
        

        // Flip character based on movement direction
        if (movement.x != 0)
        {
            spriteRenderer.flipX = movement.x < 0;
        }
    }

    private void ApplyMovement()
    {
        // Apply movement to the character
        Vector2 isoMovement = new Vector2(movement.x, movement.y * 1f); // Adjust vertical speed for isometric effect
        Vector2 newVelocity = isoMovement * speed;
        transform.position += (Vector3)newVelocity * Time.fixedDeltaTime; // Use Vector3 for position update
    }

    private void StartJump()
    {
        isJumping = true;
        animator.SetBool("isJumping", true);
        jumpTimer = 0f;
        baseYPosition = spriteTransform.localPosition.y; // Set the base Y position when the jump starts
    }

    private void EndJump()
    {
        isJumping = false;
        hasReachedPeak = false;
        animator.SetBool("isJumping", false);
        animator.SetBool("postPeak", false);
    }

    private void HandleJump()
    {
        jumpTimer += Time.deltaTime;

        // Calculate jump height based on timer (visual effect only)
        float t = jumpTimer / jumpDuration;
        float height = (-4 * Mathf.Pow(t - 0.5f, 2) + 1) * jumpHeight;

        // Update the sprite's vertical position using baseYPosition
        spriteTransform.localPosition = new Vector3(spriteTransform.localPosition.x, baseYPosition + height, spriteTransform.localPosition.z);

        // Update vertical velocity
        yVelocity = (-4 * (t - 0.5f) * jumpHeight) / jumpDuration; // Calculate current vertical velocity

        // Check for peak detection using yVelocity
        if (!hasReachedPeak && yVelocity <= 0f)
        {
            hasReachedPeak = true;
            animator.SetBool("postPeak", true);
        }

        // Switch sprite based on jump phase
        if (!hasReachedPeak) // Ascending
        {
            animator.SetBool("postPeak", false);
        }
        else // Descending
        {
            animator.SetBool("postPeak", hasReachedPeak);
        }

        // End jump after duration
        if (jumpTimer >= jumpDuration)
        {
            EndJump();
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object is in the "Water" layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            // Set the scene to level 1
            SceneManager.LoadScene("LVL_1"); // Make sure level 1 corresponds to the correct scene index
        }



        if (collision.gameObject.layer == LayerMask.NameToLayer("EndMonologue1"))
        {
            // Set the scene to level 1
            SceneManager.LoadScene("LVL_2"); // Make sure level 1 corresponds to the correct scene index
        }



        if (collision.gameObject.layer == LayerMask.NameToLayer("EndMonologue2"))
        {
            // Set the scene to level 1
            SceneManager.LoadScene("LVL_4"); // Make sure level 1 corresponds to the correct scene index
        }


        if (collision.gameObject.layer == LayerMask.NameToLayer("EndMonologue3"))
        {
            // Set the scene to level 1
            SceneManager.LoadScene("LVL_5"); // Make sure level 1 corresponds to the correct scene index
        }


        if (collision.gameObject.layer == LayerMask.NameToLayer("EndMonologue4"))
        {
            // Set the scene to level 1
            SceneManager.LoadScene("LVL_6"); // Make sure level 1 corresponds to the correct scene index
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("EndMonologue5"))
        {
            // Set the scene to level 1
            SceneManager.LoadScene(0); // Make sure level 1 corresponds to the correct scene index
        }

    }


}
