using UnityEngine;
using System.Collections; // Necesario para Coroutines
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class MargaretMovement : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] private float flySpeed = 5f;
    [SerializeField] private float swoopSpeed = 15f;
    [SerializeField] private float jumpArcHeight = 5f; // Altura del arco en el salto pisotón
    [SerializeField] private float jumpDuration = 1f; // Duración del salto pisotón

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    // [SerializeField] private Animator animator; // Opcional si el Controller lo maneja

    private Coroutine movementCoroutine;
    private bool isMoving = false;

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Asumimos que no usa gravedad normalmente
    }

    public bool IsMoving() => isMoving;

    // Mueve a una posición específica a velocidad normal
    public void StartFlyToTarget(Vector2 targetPosition, Action onArriveCallback = null)
    {
        StopCurrentMovement();
        movementCoroutine = StartCoroutine(FlyToTargetCoroutine(targetPosition, flySpeed, onArriveCallback));
    }

    // Ejecuta un dash rápido (Swoop)
    public void StartSwoop(Vector2 startPosition, Vector2 endPosition, Action onCompleteCallback = null)
    {
        StopCurrentMovement();
        transform.position = startPosition; // Posiciona instantáneamente al inicio del swoop
        movementCoroutine = StartCoroutine(SwoopCoroutine(endPosition, onCompleteCallback));
    }

     // Ejecuta el salto pisotón en arco
    public void StartJumpAttack(Vector2 targetLandPosition, Action onLandCallback = null)
    {
        StopCurrentMovement();
        movementCoroutine = StartCoroutine(JumpArcCoroutine(targetLandPosition, onLandCallback));
    }

    public void StopCurrentMovement()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        rb.velocity = Vector2.zero;
        isMoving = false;
         // Si usas gravedad para el salto, asegúrate de resetearla
        // rb.gravityScale = 0;
    }

    private IEnumerator FlyToTargetCoroutine(Vector2 targetPosition, float speed, Action onArriveCallback)
    {
        isMoving = true;
        // animator?.SetBool("IsFlying", true);

        while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            rb.velocity = direction * speed;
            // Rotar para mirar la dirección (opcional)
            // transform.right = direction;
            yield return new WaitForFixedUpdate(); // Movimiento en FixedUpdate
        }

        rb.velocity = Vector2.zero;
        transform.position = targetPosition; // Asegura la posición final
        isMoving = false;
        // animator?.SetBool("IsFlying", false);
        onArriveCallback?.Invoke();
        movementCoroutine = null;
    }

     private IEnumerator SwoopCoroutine(Vector2 targetPosition, Action onCompleteCallback)
    {
        isMoving = true;
        // animator?.SetTrigger("SwoopTrigger");
        // TODO: Activar Collider de Daño del Swoop

        Vector2 startPos = transform.position;
        float distance = Vector2.Distance(startPos, targetPosition);
        float duration = distance / swoopSpeed;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / duration;
            // Movimiento lineal simple, puedes usar curvas para más estilo
            rb.MovePosition(Vector2.Lerp(startPos, targetPosition, normalizedTime));
            yield return null; // Movimiento en Update o FixedUpdate
        }


        rb.velocity = Vector2.zero;
        transform.position = targetPosition;
        isMoving = false;
        // TODO: Desactivar Collider de Daño del Swoop
        onCompleteCallback?.Invoke();
        movementCoroutine = null;
    }


     private IEnumerator JumpArcCoroutine(Vector2 targetLandPosition, Action onLandCallback)
    {
        isMoving = true;
        // animator?.SetTrigger("JumpCharge");
        // TODO: Mostrar indicador de aterrizaje en targetLandPosition

        Vector2 startPos = transform.position;
        rb.gravityScale = 1; // O un valor que funcione con tu física 2D
        float gravity = Physics2D.gravity.magnitude * rb.gravityScale;

        // Calcular velocidad inicial para alcanzar el arco
        // Fórmula básica de trayectoria: Vx = dx / t, Vy = dy / t + 0.5 * g * t
        // Para simplificar, podemos forzar una velocidad vertical y calcular la horizontal
        float initialYVelocity = Mathf.Sqrt(2.0f * gravity * jumpArcHeight);
        float timeToPeak = initialYVelocity / gravity;
        float totalTime = timeToPeak * 2; // Asume simetría, ajustar si no

        // Si quieres que dure 'jumpDuration', recalcula velocidades (más complejo)
        // O simplemente escala la velocidad calculada
        float calculatedDuration = totalTime;
        float speedScale = calculatedDuration / jumpDuration; // Si es > 1 va más rápido, < 1 más lento

        initialYVelocity /= speedScale; // Ajusta velocidad Y
        Vector2 displacement = targetLandPosition - startPos;
        float initialXVelocity = displacement.x / jumpDuration; // Ajusta velocidad X

        rb.velocity = new Vector2(initialXVelocity, initialYVelocity);
        // animator?.SetTrigger("JumpAirborne");

        // Esperar hasta que empiece a caer (pasó el pico) o toque el suelo
        yield return new WaitUntil(() => rb.velocity.y < 0 || IsGrounded()); // Necesitas una función IsGrounded()

        // animator?.SetTrigger("JumpLand");
         // Esperar a tocar el suelo de verdad si aún no lo hizo
        yield return new WaitUntil(IsGrounded);

        rb.velocity = Vector2.zero;
        rb.gravityScale = 0; // Quita la gravedad
        transform.position = new Vector2(targetLandPosition.x, transform.position.y); // Ajusta X, deja Y donde aterrizó

        isMoving = false;
        // TODO: Ocultar indicador de aterrizaje
        onLandCallback?.Invoke(); // <-- Llamar aquí para el efecto de daño en área
        movementCoroutine = null;
    }

    // Necesitas implementar esto según tu setup (Raycast, Collider check, etc.)
    private bool IsGrounded()
    {
        // Ejemplo simple con Raycast hacia abajo
        float groundCheckDist = 0.2f;
        // Asegúrate que 'Ground' es tu layer de suelo
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDist, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }
}