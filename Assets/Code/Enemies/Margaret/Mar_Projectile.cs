using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class MargaretProjectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float damage = 25f; // Daño al jugador
    [SerializeField] private float lifetime = 5f; // Tiempo antes de destruirse

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Asegurarse de que el proyectil no sea afectado por la gravedad
        rb.gravityScale = 0f;
    }

    void Start()
    {
        // Destruir el proyectil después de su tiempo de vida
        Destroy(gameObject, lifetime);
        Debug.Log($"Projectile spawned at {transform.position}");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Comprobar si colisiona con el jugador
        if (collision.CompareTag("Player"))
        {
            // Obtener el GameManager para infligir daño
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.takeDamage(damage);
                Debug.Log($"Projectile hit player, dealing {damage} damage");
            }
            // Destruir el proyectil al impactar
            Destroy(gameObject);
        }
    }

    // Opcional: Visualizar el collider en el editor
    private void OnDrawGizmos()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, collider.bounds.extents.x);
        }
    }
}