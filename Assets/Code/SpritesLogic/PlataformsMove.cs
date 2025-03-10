using UnityEngine;

public class MovingPlatformVertical : MonoBehaviour
{
    public Transform topPoint, bottomPoint; // Puntos superior e inferior
    public float speed = 2f;
    private Vector2 targetPosition;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = topPoint.position; // Comienza moviéndose hacia arriba
    }

    void FixedUpdate()
    {
        // Mueve la plataforma de arriba a abajo
        rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime));

        // Cambia la dirección al llegar a un punto
        if (Vector2.Distance(rb.position, targetPosition) < 0f)
        {
            targetPosition = (targetPosition == (Vector2)topPoint.position) ? bottomPoint.position : topPoint.position;
        }
    }

    // Hace que el jugador se mueva con la plataforma
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}