using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovment : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform[] movePointsReference;
    public float Speed = 10.0f;
    private int pointIndex = 0;
    private Vector3[] movePoints;

    public float detectionRadius = 2.0f;

    public LayerMask playerLayer;

    private Transform target = null;

    private SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        PrepareMovePositions();
        transform.position = movePoints[pointIndex];
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForTarget();
        Move();

    }

    private void Move()
    {
        if (target != null)
        {
            
            if (Vector2.Distance(transform.position, target.position) < 0.2f)
            {
                return;
            }

            //transform.position = Vector2.MoveTowards(transform.position, target.position, Speed * Time.deltaTime);

            // Maggie debería moverse en horizontal, no?
            // - Emi
            transform.position = Vector2.MoveTowards( transform.position, new Vector2(target.position.x, transform.position.y), Speed * Time.deltaTime);

            return;


        }



        if (pointIndex <= movePoints.Length - 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, movePoints[pointIndex], Speed * Time.deltaTime);
            

            if (Vector2.Distance(transform.position, movePoints[pointIndex]) < 0.02f)
            {
                pointIndex++;
                transform.Rotate(0, 180, 0); // Aquí se voltea cuando llega a su punto
      
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
}