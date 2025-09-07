using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player_Movement : MonoBehaviour
{
    [Header("Bewegung")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;

    private Rigidbody2D rb;
    private float inputX;
    private bool jumpPressed;
    private bool facingRight = true;
    private bool grounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GetInput();
        HandleFlip();
    }

    void FixedUpdate()
    {
        // Bewegung
        rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);

        // Springen
        if (jumpPressed && grounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpPressed = false;
        }
    }

    private void GetInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;
    }

    private void HandleFlip()
    {
        if (facingRight && inputX < 0)
            Flip();
        else if (!facingRight && inputX > 0)
            Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    // -------- Ground Methode --------
    private void OnCollisionEnter2D(Collision2D other)
    {
        CheckGround(other, true);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        CheckGround(other, false);
    }

    private void CheckGround(Collision2D other, bool state)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = state;
        }
    }
}
