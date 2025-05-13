using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovment : MonoBehaviour
{
    public Transform[] movePointsReference;
    public float Speed = 10.0f;
    private int pointIndex = 0;
    private Vector3[] movePoints;

    public float detectionRadius = 2.0f;
    public LayerMask playerLayer;

    private Transform target = null;
    private SpriteRenderer spriteRenderer;

    // ✅ NUEVOS CAMPOS PARA DETECTAR EL SUELO
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    void Start()
    {
        PrepareMovePositions();
        transform.position = movePoints[pointIndex];
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        CheckForTarget();
        Move();
    }

    private void Move()
    {
        // ✅ Solo sigue al jugador si está en el suelo
        if (target != null && IsGrounded())
        {
            if (Vector2.Distance(transform.position, target.position) < 0.2f)
                return;

            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(target.position.x, transform.position.y),
                Speed * Time.deltaTime
            );

            return;
        }

        // Patrulla normal
        if (pointIndex <= movePoints.Length - 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, movePoints[pointIndex], Speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, movePoints[pointIndex]) < 0.02f)
            {
                pointIndex++;
                transform.Rotate(0, 180, 0);
            }

            if (pointIndex >= movePoints.Length)
                pointIndex = 0;
        }
    }

    private void CheckForTarget()
    {
        Collider2D targetCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        target = targetCollider != null ? targetCollider.transform : null;
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
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

        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
