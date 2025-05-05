using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(MargaretHealth))]
public class MargaretController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player; // Referencia al jugador
    [SerializeField] private GameObject projectilePrefab; // Prefab de proyectil
    [SerializeField] private Transform projectileSpawnPoint; // Punto de spawn de proyectiles
    private Rigidbody2D rb;
    private Animator animator;
    private MargaretHealth health;

    [Header("Movement Settings")]
    [SerializeField] private float flySpeed = 5f; // Velocidad de movimiento aéreo
    [SerializeField] private float hoverAmplitude = 2f; // Amplitud del movimiento de "flotar"
    [SerializeField] private float hoverFrequency = 1f; // Frecuencia del movimiento de flotar
    private Vector2 startPosition; // Posición inicial para el movimiento de flotación

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 3f; // Tiempo entre ataques
    [SerializeField] private float projectileSpeed = 10f; // Velocidad de proyectiles
    private float lastAttackTime; // Tiempo del último ataque
    private bool isAttacking = false; // Estado de ataque

    [Header("Phase Settings")]
    private int currentPhase = 1; // Fase actual (sincronizada con MargaretHealth)

    void Awake()
    {
        // Obtener componentes
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<MargaretHealth>();

        // Desactivar gravedad para que "vuele"
        rb.gravityScale = 0f;
        startPosition = transform.position;

        // Buscar al jugador si no está asignado
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        Debug.Log("Margaret initialized. Ready to fly and attack!");
    }

    void Start()
    {
        // Suscribirse a los eventos de MargaretHealth
        health.OnPhaseChanged += ChangePhase;
        health.OnDeath += HandleDeath;

        // Iniciar movimiento de flotación
        StartCoroutine(FlyingMovement());
    }

    void Update()
    {
        // Comprobar si puede atacar
        if (Time.time >= lastAttackTime + attackCooldown && !isAttacking)
        {
            StartCoroutine(ExecuteAttack());
        }
    }

    // Corrutina para el movimiento de "volar" (flotación suave)
    private IEnumerator FlyingMovement()
    {
        while (true)
        {
            // Movimiento sinusoidal para simular flotación
            float yOffset = Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
            Vector2 targetPosition = startPosition + new Vector2(0f, yOffset);

            // Mover suavemente hacia la posición objetivo
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, flySpeed * Time.deltaTime));

            Debug.Log($"Margaret flying at position: {rb.position}, Y-offset: {yOffset}");
            yield return null; // Esperar al siguiente frame
        }
    }

    // Cambiar comportamiento según la fase
    private void ChangePhase(int newPhase)
    {
        currentPhase = newPhase;
        Debug.Log($"Margaret switched to Phase {currentPhase}");

        // Ajustar parámetros según la fase
        switch (currentPhase)
        {
            case 1:
                attackCooldown = 3f;
                projectileSpeed = 10f;
                break;
            case 2:
                attackCooldown = 2f;
                projectileSpeed = 12f;
                break;
            case 3:
                attackCooldown = 1.5f;
                projectileSpeed = 15f;
                break;
        }
    }

    // Ejecutar ataques según la fase
    private IEnumerator ExecuteAttack()
    {
        isAttacking = true;
        Debug.Log($"Margaret starting attack in Phase {currentPhase}");

        switch (currentPhase)
        {
            case 1:
                // Fase 1: Disparar un proyectil básico hacia el jugador
                yield return StartCoroutine(ShootSingleProjectile());
                break;
            case 2:
                // Fase 2: Disparar tres proyectiles en abanico
                yield return StartCoroutine(ShootFanProjectiles(3, 15f));
                break;
            case 3:
                // Fase 3: Disparar proyectiles en círculo
                yield return StartCoroutine(ShootCircularProjectiles(8));
                break;
        }

        lastAttackTime = Time.time;
        isAttacking = false;
    }

    // Disparar un proyectil hacia el jugador
    private IEnumerator ShootSingleProjectile()
    {
        // TODO: Reproducir animación de ataque
        // animator.SetTrigger("Attack");

        Debug.Log("Margaret shooting single projectile");
        Vector2 direction = (player.position - projectileSpawnPoint.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Rigidbody2D projRb = projectile.GetComponent<Rigidbody2D>();
        projRb.velocity = direction * projectileSpeed;

        yield return null;
    }

    // Disparar proyectiles en abanico
    private IEnumerator ShootFanProjectiles(int count, float angleBetween)
    {
        // TODO: Reproducir animación de ataque
        // animator.SetTrigger("FanAttack");

        Debug.Log($"Margaret shooting {count} projectiles in fan pattern");
        Vector2 baseDirection = (player.position - projectileSpawnPoint.position).normalized;
        float startAngle = -(count - 1) * angleBetween / 2;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + i * angleBetween;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * baseDirection;
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            Rigidbody2D projRb = projectile.GetComponent<Rigidbody2D>();
            projRb.velocity = direction * projectileSpeed;
        }

        yield return null;
    }

    // Disparar proyectiles en círculo
    private IEnumerator ShootCircularProjectiles(int count)
    {
        // TODO: Reproducir animación de ataque
        // animator.SetTrigger("CircularAttack");

        Debug.Log($"Margaret shooting {count} projectiles in circular pattern");
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            Rigidbody2D projRb = projectile.GetComponent<Rigidbody2D>();
            projRb.velocity = direction * projectileSpeed;
        }

        yield return null;
    }

    // Manejar la muerte
    private void HandleDeath()
    {
        // TODO: Reproducir animación de muerte
        // animator.SetTrigger("Die");

        Debug.Log("Margaret has been defeated!");
        // TODO: Desactivar colliders, detener corrutinas, etc.
        gameObject.SetActive(false);
    }

    // Visualizar el punto de spawn de proyectiles en el editor
    private void OnDrawGizmos()
    {
        if (projectileSpawnPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(projectileSpawnPoint.position, 0.2f);
        }
    }
}