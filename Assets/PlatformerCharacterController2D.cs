using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float jumpForce = 700f;
    [SerializeField] private float movementSmoothing = .05f;
    [SerializeField] private bool airControl = true;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Animator animator;

    [Header("Dash Settings")]
    [SerializeField] private float dashForce = 25f;
    [SerializeField] private float dashDuration = 0.1f;
    [SerializeField] private float dashCooldown = 0.4f; // Time between dashes


    public AudioSource soundPlayer;
    public AudioClip dashSound;

    private bool isDashing = false;
    private bool canDash = true;

    private const float groundCheckRadius = .2f;
    private bool isGrounded;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private Vector3 velocity = Vector3.zero;

    public Rigidbody2D m_Rigidbody2D { get { return rb; } }

    [Header("Events")]
    public UnityEvent OnLandEvent;

    public ParticleSystem dust;
    public ParticleSystem dashDust;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void FixedUpdate()
    {
        // Check if character is grounded
        bool wasGrounded = isGrounded;
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }

        // Allow dash again when grounded
        if (isGrounded)
        {
            canDash = true;
        }
    }

    private void Dash(Vector2 direction)
    {
        if (canDash)
        {
            StartCoroutine(DashCoroutine(direction));
        }
    }

    private IEnumerator DashCoroutine(Vector2 dashDirection)
    {
        isDashing = true;
        canDash = false;

        // Disable gravity and set velocity
        rb.gravityScale = 0;
        rb.velocity = dashDirection * dashForce;

        yield return new WaitForSeconds(dashDuration);

        // Re-enable gravity and stop dash
        rb.gravityScale = 3; // Set this back to your normal gravity value
        isDashing = false;
        rb.velocity = Vector2.zero;

        // Cooldown before allowing the next dash
        yield return new WaitForSeconds(dashCooldown);
    }

    //public void Move(float move, bool jump, bool dash)
    //{
    //    if (isDashing)
    //    {
    //        return; // Skip other movement logic while dashing
    //    }

    //    if (isGrounded || airControl)
    //    {
    //        Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
    //        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

    //        if (move > 0 && !facingRight)
    //        {
    //            Flip();
    //        }
    //        else if (move < 0 && facingRight)
    //        {
    //            Flip();
    //        }
    //    }

    //    if (isGrounded && jump)
    //    {
    //        isGrounded = false;
    //        rb.AddForce(new Vector2(0f, jumpForce));
    //    }

    //    if (dash && canDash)
    //    {
    //        Vector2 dashDirection = GetDashDirection();
    //        Dash(dashDirection);
    //    }
    //}


    public void Move(float move, bool jump, bool dash)
    {
        // Allow dash to be executed regardless of other movement inputs
        if (dash && canDash)
        {
            Vector2 dashDirection = GetDashDirection();
            Dash(dashDirection);
            dashDust.Play();
            soundPlayer.clip = dashSound;
            soundPlayer.Play();
            return; // Exit the method after dashing to avoid further movement logic
        }

        if (isDashing)
        {
            return; // Skip other movement logic while dashing
        }

        if (isGrounded || airControl)
        {
            Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

            if (move > 0 && !facingRight)
            {
                Flip();
            }
            else if (move < 0 && facingRight)
            {
                Flip();
            }
        }

        if (isGrounded && jump)
        {
            isGrounded = false;
            rb.AddForce(new Vector2(0f, jumpForce));
        }
    }

    private Vector2 GetDashDirection()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // If there is any input, use it to determine the dash direction
        if (x != 0 || y != 0)
        {
            return new Vector2(x, y).normalized; // Normalize to handle diagonal dashing
        }
        // Default to facing direction if no input is given
        return facingRight ? Vector2.right : Vector2.left;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        if (IsGrounded())
        {
            dust.Play();
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }
}
