using UnityEngine;

public class MargaretProjectile : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float speed = 15f; // Aumenté la velocidad
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private bool useGravity = false;
    [SerializeField] private bool trackPlayer = true; // Nueva opción para seguir al jugador
    
    private Transform player;
    private Vector2 direction;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = useGravity ? 1 : 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Busca al jugador automáticamente si no se asignó
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        if (player != null)
        {
            CalculateInitialTrajectory();
        }
        else
        {
            Debug.LogWarning("No se encontró al jugador, el proyectil se moverá en dirección predeterminada");
            direction = transform.right; // Dirección por defecto
            rb.velocity = direction * speed;
        }
        
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        if (trackPlayer && player != null)
        {
            // Actualiza la dirección cada frame para seguir al jugador
            direction = (player.position - transform.position).normalized;
            
            // Rotación hacia el jugador
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        
        // Aplica movimiento constante
        rb.velocity = direction * speed;
    }

    public void SetTarget(Transform playerTransform)
    {
        player = playerTransform;
        if (player != null && rb != null)
        {
            CalculateInitialTrajectory();
        }
    }

    private void CalculateInitialTrajectory()
    {
        direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}