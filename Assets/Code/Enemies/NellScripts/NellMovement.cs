using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UI;
using UnityEngine;

public class NellMovement : MonoBehaviour
{
    [Header("Puntos de patrullaje")]
    public Transform[] movePointsReference;


    [Header("Info para que Nell huya")]
    public Transform player;
    public float safeDistance = 5f;

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

    private Healt enemyHealth;
    private int previousHealth;
    private NellAttack attack;


    void Start()
    {
        PrepareMovePositions();
        transform.position = movePoints[pointIndex];
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<Healt>();
        attack = GetComponent<NellAttack>();
        previousHealth = enemyHealth.currentHealt;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForTarget();
        Move();
        if(target != null){
            //aquí agrego que dispare el Nell cuando el jugador esté en el rango
            attack.Shoot();
        }
        if(previousHealth != enemyHealth.currentHealt){
            StartCoroutine(stunTime());
            previousHealth = enemyHealth.currentHealt;
        }
    }

    private void Move()
    {
        if (target != null)
        {   
            
            if (Vector2.Distance(transform.position, target.position) < 0.2f)
            {
                return;
            }
            //consigo la dirección de Nell al jugador y su distancia
            Vector2 oppositePlayerDir = (transform.position - player.position).normalized;
            float distance = Vector2.Distance(transform.position, player.position);

            //si el jugador se acerca demasiado (más que la safeDistance) Nell se comienza a 
            //alejar del jugador. Si el jugador sigue en rango de detección pero está dentro
            //de la distancia segura, Nell se detiene (a dispararle aún xd)
            if(distance < safeDistance){
                transform.position += (Vector3)oppositePlayerDir * speed * Time.deltaTime;
            }
            else{
                rb.velocity = Vector2.zero;
            }
            return;
        }

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

    IEnumerator stunTime(){
        float ogSpeed = speed;
        speed = 8;
        yield return new WaitForSeconds(3f);
        speed = ogSpeed;
    }
}
