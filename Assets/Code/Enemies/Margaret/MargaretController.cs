using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(MargaretHealth))]
[RequireComponent(typeof(Collider2D))]
public class MargaretController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject bulletHellProjectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private List<Transform> movementPoints;
    [SerializeField] private GameObject maggiePrefab;
    //[SerializeField] private GameObject powerEffect;
    [Header("Visual Effects")]
    [SerializeField] private Color flashColor = Color.yellow;
    [SerializeField] private float flashDuration = 0.3f;

    [Header("Movement Settings")]
    [SerializeField] private float flySpeed = 5f;
    [SerializeField] private float stoppingDistance = 1f;
    [SerializeField] private float movementCooldown = 5f;
    [SerializeField] private float hoverAmplitude = 0.2f;
    [SerializeField] private float hoverFrequency = 1.5f;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float projectileSpeed = 10f;

    [Header("Phase 1 Settings")]
    [SerializeField] private float phase1AttackDelay = 0.2f;
    [SerializeField] private float phase1AttackCooldown = 1.2f; // Reducido a 1.2 segundos
    [SerializeField] private int phase1BurstCount = 3; // Número de ráfagas en fase 1   
    [SerializeField] private float phase1BurstDelay = 0.3f; // Delay entre ráfagas
    [Header("Phase 1 Settings")]
    [SerializeField] private float initialAttackDelay = 0.5f; // Nuevo parámetro para el primer ataque

    private bool hasInitializedPhase1 = false; // Nuevo flag para control


    [Header("Phase 2 Settings")]
    [SerializeField] private int maggiesToSpawn = 3;
    [SerializeField] private float shieldDuration = 10f;
    [SerializeField] private float swoopSpeed = 15f;
    [SerializeField] private float swoopAttackCooldown = 8f;
    [SerializeField] private Transform phase2AttackPoint; // Nuevo punto específico para esta fase
    [SerializeField] private float timeToReachPoint = 2f; // Tiempo para llegar al punto

    [Header("Phase 3 Settings")]
    [SerializeField] private float bulletHellChargeTime = 1.5f;
    [SerializeField] private int bulletHellProjectiles = 20;
    [SerializeField] private float bulletHellRepeatTime = 10f;
    private Coroutine phase3RepeatRoutine;

    private Rigidbody2D rb;
    private Animator animator;
    private MargaretHealth health;
    private Collider2D margaretCollider;
    
    private Vector2 currentTargetPosition;
    private Vector2 hoverOffset;
    private float lastMovementTime;
    private float lastAttackTime;
    private float lastSwoopTime;
    private bool isMovingToNewPoint = false;
    private bool isAttacking = false;
    private bool isShielded = false;
    private int currentPhase = 1;
    
    private List<GameObject> spawnedMaggies = new List<GameObject>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<MargaretHealth>();
        margaretCollider = GetComponent<Collider2D>();

        rb.gravityScale = 0f;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (movementPoints == null || movementPoints.Count == 0)
            Debug.LogWarning("No movement points assigned!");

        if (margaretCollider == null)
            Debug.LogError("Missing Collider2D!");
    }

    void Start()
    {
        health.OnPhaseChanged += ChangePhase;
        health.OnDeath += HandleDeath;
        
        ChooseNewMovementTarget();
        lastMovementTime = Time.time;
        
        IgnorePlatformCollisions();
    }

    void Update()
    {
        if (!isMovingToNewPoint && Time.time >= lastMovementTime + movementCooldown)
            ChooseNewMovementTarget();
            
        if (!isAttacking && !isShielded && Time.time >= lastAttackTime + attackCooldown)
            if (!isMovingToNewPoint) 
                StartCoroutine(ExecuteAttack());
                
        CalculateHover();
    }

    void FixedUpdate()
    {
        if (isMovingToNewPoint)
            MoveToTarget();
        else
            ApplyHoverOnly();
            
        LookAtPlayer();
    }

    void IgnorePlatformCollisions()
    {
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform");
        foreach (GameObject platform in platforms)
        {
            Collider2D platformCollider = platform.GetComponent<Collider2D>();
            if (platformCollider != null)
                Physics2D.IgnoreCollision(margaretCollider, platformCollider, true);
        }
    }

    void CalculateHover()
    {
        float yOffset = Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        float xOffset = Mathf.Cos(Time.time * hoverFrequency * 0.7f) * hoverAmplitude * 0.5f;
        hoverOffset = new Vector2(xOffset, yOffset);
    }

    void ChooseNewMovementTarget()
    {
        if (movementPoints.Count == 0) return;
        
        int randomIndex = Random.Range(0, movementPoints.Count);
        currentTargetPosition = movementPoints[randomIndex].position;
        isMovingToNewPoint = true;
        lastMovementTime = Time.time;
    }

    void MoveToTarget()
    {
        Vector2 currentPosition = rb.position;
        Vector2 finalTarget = currentTargetPosition + hoverOffset;
        Vector2 directionToTarget = (finalTarget - currentPosition).normalized;
        
        rb.velocity = directionToTarget * flySpeed;
        
        if (Vector2.Distance(currentPosition, currentTargetPosition) <= stoppingDistance)
        {
            isMovingToNewPoint = false;
            rb.velocity = Vector2.zero;
        }
    }

    void ApplyHoverOnly()
    {
        Vector2 targetHoverPosition = (Vector2)transform.position + hoverOffset;
        rb.MovePosition(Vector2.Lerp(rb.position, targetHoverPosition, flySpeed * Time.fixedDeltaTime));
    }

    void LookAtPlayer()
    {
        if (player == null) return;
        
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        transform.localScale = new Vector3(
            Mathf.Abs(transform.localScale.x) * (directionToPlayer.x > 0 ? 1 : -1),
            transform.localScale.y,
            transform.localScale.z
        );
    }

        private void ChangePhase(int newPhase) 
{
    currentPhase = newPhase;
    
    if (phase3RepeatRoutine != null)
    {
        StopCoroutine(phase3RepeatRoutine);
        phase3RepeatRoutine = null;
    }
    
    switch (currentPhase) 
    {
        case 1:
            attackCooldown = phase1AttackCooldown;
            projectileSpeed = 10f;
            movementCooldown = 5f;
            isShielded = false;
            animator.SetBool("IsShielded", false);
            
            // Forzar primer ataque inmediato
            if (!hasInitializedPhase1)
            {
                hasInitializedPhase1 = true;
                StartCoroutine(InitialPhase1Attack());
            }
            break;
            
        case 2:
            attackCooldown = 2f;
            projectileSpeed = 12f;
            movementCooldown = 3f;
            StartCoroutine(Phase2Behavior());
            break;
            
        case 3:
            attackCooldown = 1.5f;
            projectileSpeed = 15f;
            movementCooldown = 2.5f;
            phase3RepeatRoutine = StartCoroutine(RepeatPhase3Attack());
            break;
    }
}

private IEnumerator InitialPhase1Attack()
{
    // Pequeña espera para permitir la inicialización
    yield return new WaitForSeconds(initialAttackDelay);
    
    // Ejecutar primer ataque sin esperar cooldown
    if (!isAttacking && !isMovingToNewPoint)
    {
        yield return StartCoroutine(Phase1AttackPattern());
    }
    
    // Reiniciar el lastAttackTime para el siguiente cooldown
    lastAttackTime = Time.time;
}

    private IEnumerator RepeatPhase3Attack()
{
    while (currentPhase == 3) // Solo mientras esté en fase 3
    {
        // Ejecutar el ataque completo
        yield return StartCoroutine(Phase3Behavior());
        
        // Esperar tiempo de repetición
        yield return new WaitForSeconds(bulletHellRepeatTime);
    }
}

    private IEnumerator ExecuteAttack()
    {
        if (isShielded || isAttacking) yield break;
        
        isAttacking = true;
        
        switch (currentPhase)
        {
            case 1:
                Debug.Log("Entro en fase 1");
                yield return StartCoroutine(Phase1AttackPattern());
                break;
                
            case 2:
            Debug.Log("Entro en fase 2");
                if (Time.time >= lastSwoopTime + swoopAttackCooldown)
                    yield return StartCoroutine(SwoopAttack());
                else
                    yield return StartCoroutine(ShootProjectiles(3, 0.2f));
                break;
                
            case 3:
            Debug.Log("Entro en fase 3");
                yield return StartCoroutine(ShootProjectiles(8, 0.1f));
                break;
        }
        
        lastAttackTime = Time.time;
        isAttacking = false;
    }

        private IEnumerator Phase1AttackPattern()
{
    isAttacking = true;
    animator.SetTrigger("Attack");
    yield return new WaitForSeconds(0.3f); // Tiempo de animación
    
    // Disparar múltiples ráfagas
    for (int i = 0; i < phase1BurstCount; i++)
    {
        yield return StartCoroutine(ShootProjectiles(3, 0.15f));
        yield return new WaitForSeconds(phase1BurstDelay);
    }
    
    yield return new WaitForSeconds(phase1AttackDelay);
    
    // Solo moverse si ha pasado el cooldown
    if (Time.time - lastMovementTime > movementCooldown)
    {
        ChooseNewMovementTarget();
    }
    
    isAttacking = false;
}

    private IEnumerator Phase2Behavior()
{
    // 1. Activar escudo y moverse al punto designado
    isShielded = true;
    health.SetPhaseInvulnerable(true);
    animator.SetBool("IsShielded", true);
    
    // Mover al punto de fase 2
    if (phase2AttackPoint != null)
    {
        currentTargetPosition = phase2AttackPoint.position;
        isMovingToNewPoint = true;
        
        float elapsedTime = 0f;
        Vector2 startingPos = transform.position;
        
        while (elapsedTime < timeToReachPoint)
        {
            transform.position = Vector2.Lerp(startingPos, currentTargetPosition, elapsedTime/timeToReachPoint);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.position = currentTargetPosition;
        isMovingToNewPoint = false;
    }

    // 2. Spawnear Maggies
    for (int i = 0; i < maggiesToSpawn; i++)
    {
        float angle = i * (360f / maggiesToSpawn);
        Vector2 spawnDir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        Vector2 spawnPos = (Vector2)transform.position + spawnDir * 3f;
        
        GameObject maggie = Instantiate(maggiePrefab, spawnPos, Quaternion.identity);
        var maggieHealth = maggie.GetComponent<Healt>();
        if (maggieHealth != null)
        {
            maggieHealth.OnDeath += () => {
                spawnedMaggies.Remove(maggie);
                CheckMaggiesDefeated();
            };
        }
        spawnedMaggies.Add(maggie);
        yield return new WaitForSeconds(0.3f);
    }

    // 3. Comportamiento mientras los Maggies estén vivos
    while (spawnedMaggies.Count > maggiesToSpawn / 2)
    {
        // Margaret se queda en su posición atacando periódicamente
        if (!isAttacking && Time.time >= lastAttackTime + attackCooldown)
        {
            yield return StartCoroutine(ShootProjectiles(3, 0.2f));
            lastAttackTime = Time.time;
        }
        
        // Pequeña espera para evitar sobrecarga
        yield return new WaitForSeconds(0.1f);
    }

    // 4. Finalizar fase 2
    isShielded = false;
    health.SetPhaseInvulnerable(false);
    animator.SetBool("IsShielded", false);
    
    // Volver a moverse normalmente
    ChooseNewMovementTarget();
}

// Nueva función para verificación
private void CheckMaggiesDefeated()
{
    Debug.Log($"Maggies restantes: {spawnedMaggies.Count}");
    
    // Opcional: Efecto visual cuando quedan pocos maggies
    if (spawnedMaggies.Count == 1)
    {
        StartCoroutine(FlashEffect(Color.red, 0.5f));
    }
}

    private IEnumerator SwoopAttack()
    {
        if (player == null) yield break;
        
        isMovingToNewPoint = true;
        animator.SetTrigger("Swoop");
        lastSwoopTime = Time.time;
        
        // First pass
        Vector2 swoopStart = (Vector2)player.position + new Vector2(
            Random.value > 0.5f ? 7f : -7f,
            5f
        );
        
        currentTargetPosition = swoopStart;
        yield return new WaitUntil(() => !isMovingToNewPoint);
        
        // Dive toward player
        rb.velocity = ((Vector2)player.position - rb.position).normalized * swoopSpeed;
        yield return new WaitForSeconds(0.5f);
        
        // Up again
        Vector2 swoopEnd = (Vector2)player.position + new Vector2(
            Random.value > 0.5f ? -7f : 7f,
            5f
        );
        
        currentTargetPosition = swoopEnd;
    }

       private IEnumerator Phase3Behavior()
{
    // Charge attack
    health.SetPhaseInvulnerable(true);
    animator.SetTrigger("Charge");
    StartCoroutine(FlashEffect(flashColor, flashDuration));
    
    // Carga progresiva con feedback visual
    float timer = 0;
    while (timer < bulletHellChargeTime)
    {
        timer += Time.deltaTime;
        // Efecto visual opcional durante la carga
        //float glowIntensity = timer / bulletHellChargeTime;
        yield return null;
    }
    
    // Bullet hell attack
    animator.SetTrigger("Release");
    for (int i = 0; i < bulletHellProjectiles; i++)
    {
        float angle = i * (360f / bulletHellProjectiles);
        Vector2 dir = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        );
        
        GameObject proj = Instantiate(
            bulletHellProjectilePrefab,
            (Vector2)transform.position + dir * 2f,
            Quaternion.identity
        );
        
        // Configuración del proyectil simplificado
        var projectile = proj.GetComponent<MargaretBulletHellProjectile>();
        if (projectile != null)
        {
            projectile.SetDirection(dir); // Usa el método específico
        }
        else // Fallback por si usamos prefab equivocado
        {
            Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
            if (projRb != null) projRb.velocity = dir * projectileSpeed;
        }
        
        yield return new WaitForSeconds(0.1f); // Espaciado entre proyectiles
    }
    
    health.SetPhaseInvulnerable(false);
}
    private IEnumerator ShootProjectiles(int count, float delay)
{
    if (projectilePrefab == null || projectileSpawnPoint == null || player == null)
        yield break;

    animator.SetTrigger("Attack");
    yield return new WaitForSeconds(0.3f); // Sync con animación

    for (int i = 0; i < count; i++)
    {
        Vector2 direction = (player.position - projectileSpawnPoint.position).normalized;
        GameObject proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        
        Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
        if (projRb != null) 
        {
            projRb.velocity = direction * projectileSpeed;
            
            // Rotación opcional hacia el jugador
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            proj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
        yield return new WaitForSeconds(delay);
    }
}

    private void HandleDeath()
    {
        Debug.Log("Margaret defeated!");
        StopAllCoroutines();
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        margaretCollider.enabled = false;
        this.enabled = false;
    }

    void OnDrawGizmos()
    {
        if (projectileSpawnPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(projectileSpawnPoint.position, 0.2f);
        }
        
        if (movementPoints != null)
        {
            Gizmos.color = Color.cyan;
            foreach (Transform point in movementPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawWireSphere(point.position, 0.5f);
                    Gizmos.DrawLine(transform.position, point.position);
                }
            }
        }
    }

    private IEnumerator FlashEffect(Color targetColor, float duration)
{
    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
    if (spriteRenderer == null) yield break;
    
    Color originalColor = spriteRenderer.color;
    spriteRenderer.color = targetColor;
    
    yield return new WaitForSeconds(duration);
    
    spriteRenderer.color = originalColor;
}

private IEnumerator BlinkEffect(int blinks, float durationPerBlink)
{
    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
    if (spriteRenderer == null) yield break;
    
    Color originalColor = spriteRenderer.color;
    
    for (int i = 0; i < blinks; i++)
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(durationPerBlink/2);
        spriteRenderer.color = originalColor;
        yield return new WaitForSeconds(durationPerBlink/2);
    }
}
}