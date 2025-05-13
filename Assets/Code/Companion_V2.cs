using UnityEngine;

public class Companion_V2 : MonoBehaviour
{
    public GameObject player;
    public float speed = 3f;
    public float followDistance = 1f;
    public float deathDistance = 100f;
    public float jumpForce = 5f;
    public float reviveDistance = 2f;
    public LayerMask groundLayer;
    public GameObject ghost;

    private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer ghostSpriteRenderer;
    private bool isGrounded;
    public bool isDead;

    private float groundCheckDistance = 1.2f;
    private float obstacleCheckDistance = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isDead = false;

        // Asegura visibilidad correcta desde el inicio
        spriteRenderer.enabled = true;
        ghostSpriteRenderer.enabled = false;

        Collider2D col1 = GetComponent<Collider2D>();
        Collider2D col2 = player.GetComponent<Collider2D>();

        if (col1 != null && col2 != null)
        {
            Physics2D.IgnoreCollision(col1, col2, true);
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        float distanceGhostToPlayer = Vector2.Distance(ghost.transform.position, player.transform.position);

        if (!isDead)
        {
            Vector2 direction = player.transform.position - transform.position;
            float horizontalDistance = direction.x;

            if (Mathf.Abs(horizontalDistance) > followDistance)
            {
                float moveDirection = Mathf.Sign(horizontalDistance);
                rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);

                // Orientar sprite
                if (moveDirection > 0)
                {
                    spriteRenderer.flipX = false;
                    ghostSpriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                    ghostSpriteRenderer.flipX = true;
                }
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            /*
            if (distanceToPlayer > deathDistance)
            {
                Die();
            }*/

            // Comprobación de suelo
            isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);

            // Detectar si hay un obstáculo enfrente
            if (isGrounded && CheckForObstacle())
            {
                Jump();
            }
        }
        else
        {
            if (distanceGhostToPlayer < reviveDistance)
            {
                HandleReviveState();
            }
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    bool CheckForObstacle()
    {
        if (Mathf.Abs(rb.velocity.x) < 0.1f) return false;

        Vector2 direction = Vector2.right * Mathf.Sign(rb.velocity.x);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up * 0.5f, direction, obstacleCheckDistance, groundLayer);
        Debug.DrawRay(transform.position + Vector3.up * 0.5f, direction * obstacleCheckDistance, Color.blue);

        return hit.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hazard") || collision.CompareTag("Enemy"))
        {
            Die();
        }
    }

    public void Die()
    {
        if (ghost != null)
        {
            isDead = true;
            ghost.transform.position = transform.position;
            HideSprite();
        }
    }

    private void HideSprite()
    {
        spriteRenderer.enabled = false;
        ghostSpriteRenderer.enabled = true;
    }

    private void MakeOpaque()
    {
        spriteRenderer.enabled = true;
        ghostSpriteRenderer.enabled = false;
    }

    public void HandleReviveState()
    {
        isDead = false;
        MakeOpaque();
    }
}
