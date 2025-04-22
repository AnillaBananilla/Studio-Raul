using UnityEngine;
using System.Collections;

public class MargaretAttack_Jump : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MargaretController controller;
    [SerializeField] private MargaretMovement movement;
    [SerializeField] private GameObject stompAreaIndicatorPrefab; // Prefab del círculo/sombra que crece
    [SerializeField] private GameObject stompDamageAreaPrefab; // Prefab invisible con collider y script AoEDamage

    [Header("Attack Settings")]
    [SerializeField] private float indicatorGrowTime = 0.8f;
    [SerializeField] private float delayBeforeNextJump = 0.5f;

    private int jumpsRemaining;
    private GameObject currentIndicator;

    public void StartAttackSequence(int numberOfJumps)
    {
         if (controller.CurrentState == MargaretController.BossState.Dead) return;
        jumpsRemaining = numberOfJumps;
        StartCoroutine(JumpSequenceCoroutine());
    }

    private IEnumerator JumpSequenceCoroutine()
    {
        while(jumpsRemaining > 0)
        {
             if (controller.CurrentState == MargaretController.BossState.Dead) yield break;

            // --- Fase de Preparación ---
            Vector2 targetPos = controller.GetPlayerTransform().position; // Apunta a donde está el jugador AHORA

            // Mostrar Indicador
            if(stompAreaIndicatorPrefab != null)
            {
                currentIndicator = Instantiate(stompAreaIndicatorPrefab, targetPos, Quaternion.identity);
                // TODO: Animar el indicador (escalar, cambiar color/intensidad) durante indicatorGrowTime
                StartCoroutine(ScaleIndicator(currentIndicator, indicatorGrowTime));
            }

             // animator?.SetTrigger("JumpCharge");
             // TODO: Play Jump Charge SFX
             yield return new WaitForSeconds(indicatorGrowTime); // Espera mientras el indicador crece

             if (controller.CurrentState == MargaretController.BossState.Dead)
             {
                 if(currentIndicator != null) Destroy(currentIndicator);
                 yield break;
             }

             // --- Fase de Salto y Caída ---
             // animator?.SetTrigger("JumpExecute");
             bool landed = false;
             movement.StartJumpAttack(targetPos, () => {
                 // --- Fase de Impacto (Callback de Movement) ---
                 // animator?.SetTrigger("JumpLand");
                 // TODO: Play Impact SFX/VFX (polvo, temblor pantalla?)

                 // Crear área de daño
                 if (stompDamageAreaPrefab != null)
                 {
                     GameObject damageArea = Instantiate(stompDamageAreaPrefab, transform.position, Quaternion.identity);
                     // El script AoEDamage en el prefab hará el resto
                     Destroy(damageArea, 0.2f); // Destruye el área de daño rápido
                 }

                // Destruir indicador si existe
                if(currentIndicator != null) Destroy(currentIndicator);

                 landed = true; // Marcar que aterrizó
             });


             // Esperar a que el callback de aterrizaje se ejecute
             yield return new WaitUntil(() => landed);


             jumpsRemaining--;

             if (jumpsRemaining > 0)
             {
                  yield return new WaitForSeconds(delayBeforeNextJump); // Pausa antes del siguiente salto
             }
        }

        // Terminaron todos los saltos
        controller.OnAttackComplete(true); // Entrar en Stun después del último salto
    }

     // Corutina simple para escalar el indicador
    private IEnumerator ScaleIndicator(GameObject indicatorInstance, float duration)
    {
         if (indicatorInstance == null) yield break;
         Transform indicatorTransform = indicatorInstance.transform;
         Vector3 initialScale = Vector3.zero; // Empieza pequeño
         Vector3 targetScale = indicatorTransform.localScale; // Escala final definida en el prefab
         indicatorTransform.localScale = initialScale;

         float timer = 0f;
         while(timer < duration)
         {
             if (indicatorInstance == null) yield break; // Si se destruyó antes
             timer += Time.deltaTime;
             indicatorTransform.localScale = Vector3.Lerp(initialScale, targetScale, timer / duration);
             // Podrías también cambiar color/alpha aquí
             yield return null;
         }
         if (indicatorInstance != null)
            indicatorTransform.localScale = targetScale; // Asegura escala final
    }
}