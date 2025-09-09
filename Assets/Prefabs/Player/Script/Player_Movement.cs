using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player_Movement : MonoBehaviour
{
    #region Instanzen
    [Header("Bewegung")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;

    private Rigidbody2D rb;
    private float inputX;
    private bool jumpPressed;
    private bool facingRight = true;
    private bool grounded;
    #endregion

    #region Awake
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    #endregion

    #region Update
    void Update()
    {
        GetInput();
        HandleFlip();
    }
    #endregion

    #region FixedUpdate
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
    #endregion

    #region GetInput
    private void GetInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;
    }
    #endregion

    #region HandleFlip
    private void HandleFlip()
    {
        if (facingRight && inputX < 0)
            Flip();
        else if (!facingRight && inputX > 0)
            Flip();
    }
    #endregion

    #region Flip
    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
    #endregion

    #region Ground
    // -------- Ground Methode --------

    #region OnCollisionEnter2D
    private void OnCollisionEnter2D(Collision2D other)
    {
        CheckGround(other, true);
    }
    #endregion

    #region OnCollisionExit2D
    private void OnCollisionExit2D(Collision2D other)
    {
        CheckGround(other, false);
    }
    #endregion

    #region CheckGround
    private void CheckGround(Collision2D other, bool state)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = state;
        }
    }
    #endregion

    #endregion
}
