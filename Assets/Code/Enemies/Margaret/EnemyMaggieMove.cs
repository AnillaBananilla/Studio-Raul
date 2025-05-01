using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float attackRange = 1.5f;
    public float detectionRange = 5.0f;
    public LayerMask playerLayer;
    
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private bool isAttacking = false;
    private float attackCooldown = 2.0f;
    private float lastAttackTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Perseguir al jugador si está en rango de detección
        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            ChasePlayer();
        }
        // Atacar si está en rango de ataque
        else if (distanceToPlayer <= attackRange)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }

        // Voltear el sprite según la dirección del jugador
        FlipSprite();
    }

    void ChasePlayer()
    {
        // Moverse solo en el eje X, manteniendo la posición Y
        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void Attack()
    {
        // Implementa aquí tu lógica de ataque
        Debug.Log("Enemigo atacando al jugador!");
        
        // Ejemplo: Daño al jugador
        if (Physics2D.OverlapCircle(transform.position, attackRange, playerLayer))
        {
            player.GetComponent<Healt>()?.Damage(1);
        }
    }

    void FlipSprite()
    {
        if (player.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false; // Jugador a la derecha
        }
        else
        {
            spriteRenderer.flipX = true; // Jugador a la izquierda
        }
    }

    void OnDrawGizmosSelected()
    {
        // Rango de detección
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Rango de ataque
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}