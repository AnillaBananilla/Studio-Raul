using UnityEngine;
using System.Collections;

public class MargaretAttack_Swoop : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MargaretController controller;
    [SerializeField] private MargaretMovement movement;
    [SerializeField] private Collider2D damageCollider; // Collider que se activa durante el swoop

    [Header("Attack Settings")]
    [SerializeField] private float swoopDamage = 20f;
    [SerializeField] private float delayBeforeSwoop = 0.5f; // Tiempo fuera de pantalla

    void Awake()
    {
        if(damageCollider != null) damageCollider.enabled = false; // Asegura que empieza desactivado
    }

    public void StartAttack(Vector2 startPos, Vector2 endPos)
    {
         if (controller.CurrentState == MargaretController.BossState.Dead) return;
        StartCoroutine(SwoopSequenceCoroutine(startPos, endPos));
    }

    private IEnumerator SwoopSequenceCoroutine(Vector2 startPos, Vector2 endPos)
    {
        // 1. Mover a la posición inicial (fuera de pantalla)
        transform.position = startPos; // Teleport rápido
        // animator?.SetTrigger("FlyOffscreenTrigger"); // Opcional

        yield return new WaitForSeconds(delayBeforeSwoop);

         if (controller.CurrentState == MargaretController.BossState.Dead) yield break;

        // 2. Ejecutar Swoop
        // animator?.SetTrigger("SwoopTrigger");
        if(damageCollider != null) damageCollider.enabled = true; // Activar daño
        // TODO: Play Swoop SFX

        bool swoopComplete = false;
        movement.StartSwoop(startPos, endPos, () => {
            swoopComplete = true; // Marcar como completo en el callback
        });

        yield return new WaitUntil(() => swoopComplete); // Esperar a que termine el movimiento

        if(damageCollider != null) damageCollider.enabled = false; // Desactivar daño

        // 3. Terminar
        controller.OnAttackComplete(false); // Volver a Idle (o decidir otra cosa)
    }

    // --- Manejo de Daño del Collider ---
    void OnTriggerEnter2D(Collider2D other)
    {
        // Solo dañar si el collider está activo (durante el swoop)
        if (damageCollider != null && damageCollider.enabled && other.CompareTag("Player"))
        {
            // Asume que el jugador tiene vida en GameManager
            /*PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(swoopDamage);
                // TODO: Play Hit Player SFX/VFX
            }*/
        }
    }
}