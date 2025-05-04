using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ArbolilloMovement : MonoBehaviour
{
    [Header("Puntos de patrullaje")]
    public Transform[] movePointsReference;


    [Header("Info para Localizar al jugador")]
    public Transform player;
    public Healt playerHP;
    private Vector3 direction;


    [Header("Movement")]
    public float speed = 10.0f;
    private int pointIndex = 0;
    private Vector3[] movePoints;
    public Rigidbody2D rb;


    [Header("Detection")]
    public float detectionRadius = 13.0f;

    public LayerMask playerLayer;

    private Transform target = null;

    private SpriteRenderer spriteRenderer;
    public float Recoilforce;
    
    //Referencias de salud
    private Healt enemyHealth;
    private int previousHealth;

    [Header("Timing Enterrado")]
    public float undergroundDuration = 3f;
    public float stayActiveAfterEmerging = 2f;
    public float emergeDistanceFromPlayer = 1f;
    public bool canTakeDamage = true;
    
    [Header("Partículas de tierra")]
    public ParticleSystem dustParticles;

    //estados arbolillo y collider
    private enum EnemyState { Pasivo, Enterrado, Emergido }
    private EnemyState currentState = EnemyState.Pasivo;
    private bool hasBeenAggroed = false;
    private CapsuleCollider2D arbolliloCollider;


    void Start()
    {
        PrepareMovePositions();
        transform.position = movePoints[pointIndex];
        spriteRenderer = GetComponent<SpriteRenderer>();
        arbolliloCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<Healt>();
        previousHealth = enemyHealth.currentHealt;
    }
    void Update()
    {
        CheckDamage();
        previousHealth = enemyHealth.currentHealt;
        if(currentState == EnemyState.Pasivo){
            CheckForTarget();
            Patrol();
        }
    }

    private void CheckDamage(){
        if(currentState == EnemyState.Pasivo && previousHealth != enemyHealth.currentHealt){
            hasBeenAggroed = true;
            StartCoroutine(UndergroundLoop());
        }
    }
    private void Patrol()
    {
        if (pointIndex <= movePoints.Length - 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, movePoints[pointIndex], speed * Time.deltaTime);
            

            if (Vector2.Distance(transform.position, movePoints[pointIndex]) < 0.02f)
            {
                pointIndex++;
                transform.Rotate(0, 180, 0);
      
            }
            if (pointIndex >= movePoints.Length)
            {
                pointIndex = 0;
            }
        }
    }

    private void CheckForTarget()
    {
        Collider2D targetCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (targetCollider != null)
        {
           
            target = targetCollider.gameObject.transform;
        }
        else
        {
            target = null;

        }

    }
    private void PrepareMovePositions()
    {
        movePoints = new Vector3[movePointsReference.Length];

        for (int i = 0; i < movePointsReference.Length; i++)
        {
            movePoints[i] = movePointsReference[i].position;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    //método de comportamiento 
    IEnumerator GoUnderground(){
        currentState = EnemyState.Enterrado;
        canTakeDamage = false;

        // Desactivar sprite y colisión
        spriteRenderer.enabled = false;
        arbolliloCollider.enabled = false;

        if (dustParticles != null)
        {
            dustParticles.Play();
        }

        float elapsed = 0f;
        //tiempo antes de que se guarde la posición del jugador
        float delayBeforeLockTarget = 0.1f;

        float groundY = transform.position.y;
        Vector2 targetPosition = transform.position;

        while (elapsed < undergroundDuration)
        {   
            if(elapsed >= delayBeforeLockTarget && targetPosition == (Vector2)transform.position){
                targetPosition = player != null ? new Vector2(player.position.x, groundY): new Vector2(transform.position.x, groundY);
            }

            Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;
            // Movimiento hacia la posición guardada, NO actualizada en tiempo real y HORIZONTAL
            Vector2 nextPosition = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            nextPosition.y = groundY;
            transform.position = nextPosition;
            if (dustParticles != null)
            {
                float angle = Mathf.Atan2(-moveDirection.y, -moveDirection.x) * Mathf.Rad2Deg;
                dustParticles.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Detener partículas
        if (dustParticles != null)
        {
            dustParticles.Stop();
        }
        // Emerger cerca de la posición guardada
        Vector2 direction = ((Vector2)transform.position - targetPosition).normalized;
        Vector2 emergePosition = targetPosition + direction * emergeDistanceFromPlayer;
        emergePosition.y = groundY;
        transform.position = emergePosition;

        // Reactivar sprite y colisión
        spriteRenderer.enabled = true;
        arbolliloCollider.enabled = true;

        currentState = EnemyState.Emergido;
        canTakeDamage = true;
        hasBeenAggroed = false;

        yield return new WaitForSeconds(stayActiveAfterEmerging);

    }

    IEnumerator UndergroundLoop(){
        while(true){
            yield return StartCoroutine(GoUnderground());
            yield return new WaitForSeconds(1f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(currentState == EnemyState.Emergido && collision.CompareTag("Player")){
            playerHP.Damage(15);
            direction = (collision.transform.position - transform.position).normalized;
            direction.y = 0.05f;
            Debug.LogWarning(direction);
            collision.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
            Debug.LogWarning(direction * Recoilforce);
            collision.GetComponent<Rigidbody2D>().velocity = direction * Recoilforce;
            
        }
    }
}
