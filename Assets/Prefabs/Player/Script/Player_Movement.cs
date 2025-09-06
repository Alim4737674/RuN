using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))] // oder BoxCollider2D
public class Player_Movement : MonoBehaviour
{
    [Header("Bewegung")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground Layer")]
    [SerializeField] private LayerMask groundLayer; // z.B. "Ground"

    private Rigidbody2D rb;
    private CapsuleCollider2D col; // dein "Controller" für Bodencheck
    private float inputX;
    private bool jumpPressed;
    private bool facingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        GetInput();
        HandleFlip();
    }

    void FixedUpdate()
    {
        bool isGrounded = CheckGrounded();

        // Bewegung
#if UNITY_600_0_OR_NEWER
        rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);
#else
        rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);
#endif

        // Springen nur am Boden
        if (jumpPressed && isGrounded)
        {
#if UNITY_600_0_OR_NEWER
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
#else
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
#endif
        }

        jumpPressed = false;
    }

    private void GetInput()
    {
        inputX = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) inputX = -1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) inputX = 1f;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            jumpPressed = true;
    }

    private void HandleFlip()
    {
        if (inputX > 0 && !facingRight) Flip();
        else if (inputX < 0 && facingRight) Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        var s = transform.localScale; s.x *= -1f; transform.localScale = s;
    }

    // === "CharacterController-Style" GroundCheck in 2D ===
    private bool CheckGrounded()
    {
        // collider berührt irgendeinen Collider im Ground-Layer?
        return col.IsTouchingLayers(groundLayer);
    }
}
