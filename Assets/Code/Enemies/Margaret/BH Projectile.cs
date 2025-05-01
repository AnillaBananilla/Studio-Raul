using UnityEngine;

public class MargaretBulletHellProjectile : MonoBehaviour
{
    [Header("Basic Settings")]
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float rotationSpeed = 200f;
    
    private Vector2 direction;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Configuración física simplificada
        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        
        // Rotación inicial basada en la dirección
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        
        // Movimiento inicial
        rb.velocity = direction * speed;
        
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        // Solo rotación visual (opcional)
        transform.Rotate(0, 0, rotationSpeed * Time.fixedDeltaTime);
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        
        if (rb != null)
        {
            rb.velocity = direction * speed;
            
            // Actualizar rotación si cambia la dirección
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Destruir al chocar con cualquier cosa excepto jugador y otros enemigos
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}