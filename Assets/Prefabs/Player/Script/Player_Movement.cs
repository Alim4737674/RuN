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
    // Erstellt/Verknüpft wichtige Variablen und Objekte (z. B. Rigidbody).

    #region Awake
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    #endregion
    // Wird beim Start ausgeführt, initialisiert Komponenten.

    #region Update
    void Update()
    {
        GetInput();
        HandleFlip();
    }
    #endregion
    // Läuft jeden Frame, verarbeitet Eingaben und Logik.

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
    // Läuft in festen Zeitabständen, für Physik (Bewegung, Sprünge).

    #region GetInput
    private void GetInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;
    }
    #endregion
    // Liest die Tastatureingaben (z. B. Bewegung, Sprung).

    #region HandleFlip
    private void HandleFlip()
    {
        if (facingRight && inputX < 0)
            Flip();
        else if (!facingRight && inputX > 0)
            Flip();
    }
    #endregion
    // Prüft, ob die Figur umgedreht werden muss.

    #region Flip
    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
    #endregion
    // Dreht die Spielfigur optisch in die andere Richtung.

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
    // Prüft, ob der Spieler den Boden berührt (Ground Check).
}
