using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ProjectileMAG : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f; // Tiempo antes de autodestruirse
    [SerializeField] private float damage = 10f;
    [SerializeField] private bool isHoming = false;
    [SerializeField] private GameObject impactVFX; // Efecto al chocar

    private Rigidbody2D rb;
    private Transform target; // Para homing
    private float speed;
    private float turnSpeed; // Para homing

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponent<Collider2D>().isTrigger = true; // Asegurarse que sea trigger
    }

    void Start()
    {
        // Autodestrucción si no usa pooling
        // Si usas pooling, lo devuelves al pool en lugar de Destroy
        Destroy(gameObject, lifeTime);
    }

    public void Initialize(Transform targetPlayer, float projectileSpeed, float projectileTurnSpeed)
    {
        this.target = targetPlayer;
        this.speed = projectileSpeed;
        this.turnSpeed = projectileTurnSpeed;
        this.isHoming = true;

        // Dirección inicial
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

     public void Initialize(Vector2 direction, float projectileSpeed) // Para no guiados (Bullet Hell)
    {
        this.isHoming = false;
        this.speed = projectileSpeed;
        rb.velocity = direction.normalized * speed;
    }


    void FixedUpdate()
    {
        if (isHoming && target != null)
        {
            Vector2 directionToTarget = ((Vector2)target.position - rb.position).normalized;
            // Rotar la velocidad actual hacia el target
            Vector2 currentVelocity = rb.velocity.normalized;
            float angleDifference = Vector2.SignedAngle(currentVelocity, directionToTarget);

            // Limitar cuánto puede girar por frame
            float maxTurnAngle = turnSpeed * Time.fixedDeltaTime;
            float turnAngle = Mathf.Clamp(angleDifference, -maxTurnAngle, maxTurnAngle);

            // Aplicar rotación
            Quaternion rotation = Quaternion.AngleAxis(turnAngle, Vector3.forward); // Rotar en Z para 2D
            rb.velocity = rotation * rb.velocity;

             // Mantener la velocidad constante
             rb.velocity = rb.velocity.normalized * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Ignorar otros proyectiles o al propio jefe
        if (other.CompareTag("Enemy") || other.CompareTag("Projectile")) return; // Asegúrate de tagear bien

        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
        }

        // Crear efecto de impacto y destruir (o devolver al pool)
        if (impactVFX != null) Instantiate(impactVFX, transform.position, Quaternion.identity);
         Destroy(gameObject); // O: gameObject.SetActive(false); PoolManager.Instance.ReturnProjectile(gameObject);
    }
}