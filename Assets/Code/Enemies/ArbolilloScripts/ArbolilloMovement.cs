using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbolilloMovement : MonoBehaviour
{
    private enum EnemyState { Patrolling, Burrowing, Emerging }
    private EnemyState currentState = EnemyState.Patrolling;

    [Header("Puntos de patrullaje")]
    public Transform[] movePointsReference;

    [Header("Movement")]
    public float speed = 5.0f;
    private int pointIndex = 0;
    private Vector3[] movePoints;
    public Rigidbody2D rb;

    [Header("Detection")]
    public float detectionRadius = 13.0f;
    public LayerMask playerLayer;
    private Transform target = null;

    [Header("Burrow Behaviour")]
    public bool isAttacked = false;
    public float burrowDuration = 0.5f;
    public float distanceInFrontOfPlayer = 1.5f;
    public float emergeDuration = 0.3f;

    private Collider2D enemyCollider;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Healt enemyHealth;
    private int previousHealth;

    void Start()
    {
        PrepareMovePositions();
        transform.position = movePoints[pointIndex];

        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<Healt>();
        previousHealth = enemyHealth.currentHealt;
    }

    void Update()
    {
        CheckForTarget();
        DetectIfAttacked();

        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                break;
            case EnemyState.Burrowing:
                break;
            case EnemyState.Emerging:
                // Optional: delay or reset
                break;
        }
    }

    private void Patrol()
    {
        if (pointIndex <= movePoints.Length - 1)
        {
            Vector3 patrolTarget = new Vector3(movePoints[pointIndex].x, transform.position.y, transform.position.z);
            transform.position = Vector2.MoveTowards(transform.position, patrolTarget, speed * Time.deltaTime);

            if (Mathf.Abs(transform.position.x - patrolTarget.x) < 0.02f)
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
            target = targetCollider.transform;
        }
        else
        {
            target = null;
        }
    }

    private void DetectIfAttacked()
    {
        if (previousHealth != enemyHealth.currentHealt)
        {
            Debug.Log("Arbolillo was attacked!");
            isAttacked = true;
            previousHealth = enemyHealth.currentHealt;

            if (target != null && currentState == EnemyState.Patrolling)
            {
                currentState = EnemyState.Burrowing;
                StartCoroutine(BurrowTowardPlayer());
            }
        }
    }

    private IEnumerator BurrowTowardPlayer()
    {
        // Become invulnerable
        enemyCollider.enabled = false;
        
        // Optional visual cue
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        animator.SetTrigger("Burrow");

        yield return new WaitForSeconds(burrowDuration);

        if (target != null)
        {
            float groundY = transform.position.y;
            Vector3 inFrontOfPlayer = target.position + new Vector3(target.localScale.x * distanceInFrontOfPlayer, 0, 0);

            while (Vector2.Distance(transform.position, inFrontOfPlayer) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector3(inFrontOfPlayer.x, groundY, transform.position.z), speed * Time.deltaTime);
                yield return null;
            }
        }

        // Reappear
        animator.SetTrigger("Emerge");
        spriteRenderer.color = new Color(1, 1, 1, 1f);

        yield return new WaitForSeconds(emergeDuration);

        enemyCollider.enabled = true;
        currentState = EnemyState.Emerging;

        // Optional: return to patrolling or stay in combat
        yield return new WaitForSeconds(1f);
        currentState = EnemyState.Patrolling;
        isAttacked = false;
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
}
