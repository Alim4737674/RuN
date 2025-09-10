using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Player_Movement : MonoBehaviour
{
    #region Instanzen
    #region Inspector: Bewegung
    [Header("Bewegung")]
    [SerializeField] private float moveSpeed = 7f;   // wie schnell der Spieler läuft
    [SerializeField] private float jumpForce = 12f;  // wie hoch er springt
    #endregion

    #region Inspector: Ground Check
    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;        // Layer für Boden, damit Cast nur Boden trifft
    [SerializeField] private float groundCheckExtra = 0.05f; // minimal unter den Collider prüfen
    #endregion

    #region Komponenten
    private Rigidbody2D rb;     // Physik vom Player
    private Collider2D col;     // Collider vom Player
    private Animator animator;  // für Animationen
    #endregion

    #region State
    private float inputX;             // Input links/rechts
    private bool jumpPressed;         // ob Sprungtaste gedrückt wurde
    private bool facingRight = true;  // schaut der Player nach rechts?
    private bool grounded;            // steht der Player am Boden?
    #endregion

    #endregion

    #region Unity: Awake
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   // Rigidbody holen
        col = GetComponent<Collider2D>();   // Collider holen
        animator = GetComponent<Animator>(); // Animator holen
    }
    #endregion

    #region Unity: Update
    void Update()
    {
        GetInput();     // Eingaben abfragen
        HandleFlip();   // ggf. umdrehen

        grounded = IsGrounded(); // checken ob Player am Boden ist

        // Animationen wechseln je nach Zustand
        if (Mathf.Abs(inputX) > 0.1f && grounded)
            animator.Play("Player_Run");   // laufen
        else if (grounded)
            animator.Play("Player_Idle");  // idle
    }
    #endregion

    #region Unity: FixedUpdate
    void FixedUpdate()
    {
        // horizontale Bewegung
        rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);

        // springen nur wenn Boden berührt wird
        if (jumpPressed && grounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpPressed = false; // zurücksetzen, damit er nicht spammt
        }
    }
    #endregion

    #region Input
    private void GetInput()
    {
        inputX = Input.GetAxisRaw("Horizontal"); // A/D oder Pfeiltasten
        if (Input.GetButtonDown("Jump"))         // Space gedrückt
            jumpPressed = true;
    }
    #endregion

    #region Facing/Flip
    private void HandleFlip()
    {
        if (facingRight && inputX < 0) Flip();        // nach links drehen
        else if (!facingRight && inputX > 0) Flip();  // nach rechts drehen
    }

    private void Flip()
    {
        facingRight = !facingRight;     // Richtung merken
        transform.Rotate(0f, 180f, 0f); // optisch umdrehen
    }
    #endregion

    #region GroundCheck (BoxCast)
    // Stabiler als 1 einzelner Ray: wir "boxen" minimal nach unten.
    private bool IsGrounded()
    {
        Bounds b = col.bounds; // Collider-Grenzen
        Vector2 size = new Vector2(b.size.x , b.size.y); // etwas schmaler, reduziert Kantenfehler
        float castDistance = groundCheckExtra; // Tiefe des Checks

        // BoxCast nach unten, nur gegen groundLayer
        RaycastHit2D hit = Physics2D.BoxCast(b.center, size, 0f, Vector2.down, castDistance, groundLayer);

        #region Debug-Visualisierung
        // drei Debug-Linien im Scene-View, damit man die Check-Tiefe sieht
        Debug.DrawRay(new Vector2(b.center.x - size.x / 2f, b.min.y), Vector2.down * castDistance, Color.green);
        Debug.DrawRay(new Vector2(b.center.x + size.x / 2f, b.min.y), Vector2.down * castDistance, Color.green);
        Debug.DrawRay(new Vector2(b.center.x, b.min.y), Vector2.down * castDistance, Color.green);
        #endregion

        return hit.collider != null; // true = Boden getroffen
    }
    #endregion
}
