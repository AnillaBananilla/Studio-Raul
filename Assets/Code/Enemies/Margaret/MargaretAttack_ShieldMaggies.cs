using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Para List

public class MargaretAttack_ShieldMaggies : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MargaretController controller;
    [SerializeField] private MargaretHealth health;
    [SerializeField] private Animator animator; // Para animación de escudo

    [Header("Maggie Settings")]
    [SerializeField] private GameObject maggiePrefab;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private int numberOfMaggies = 5;
    [SerializeField] [Range(0f, 1f)] private float requiredDefeatRatio = 0.8f; // 80%

    private List<MaggieController> activeMaggies = new List<MaggieController>();
    private int maggiesToDefeat;
    private int maggiesDefeatedCount;

    public void StartAttack()
    {
         if (controller.CurrentState == MargaretController.BossState.Dead) return;
        StartCoroutine(ShieldSequenceCoroutine());
    }

    private IEnumerator ShieldSequenceCoroutine()
    {
        // 1. Entrar en modo escudo
        health.SetInvulnerable(true);
        animator?.SetTrigger("ShieldStartTrigger");
        // TODO: Play Shield Start SFX/VFX

        yield return new WaitForSeconds(1.0f); // Tiempo para que se active el escudo visualmente

         if (controller.CurrentState == MargaretController.BossState.Dead)
         {
              health.SetInvulnerable(false); // Asegurarse de quitar invulnerabilidad si muere
              yield break;
         }

        // 2. Spawnear Maggies
        activeMaggies.Clear();
        maggiesDefeatedCount = 0;
        maggiesToDefeat = Mathf.CeilToInt(numberOfMaggies * requiredDefeatRatio);

        for (int i = 0; i < numberOfMaggies; i++)
        {
            Transform spawnPoint = (spawnPoints != null && spawnPoints.Count > i) ? spawnPoints[i] : transform; // Usa puntos o la pos de Margaret
            GameObject maggieGO = Instantiate(maggiePrefab, spawnPoint.position, Quaternion.identity); // USA POOLING!
            MaggieController maggie = maggieGO.GetComponent<MaggieController>();
            if(maggie != null)
            {
                maggie.Initialize(this); // Pasa referencia a este script para notificación
                activeMaggies.Add(maggie);
            } else {
                 Destroy(maggieGO); // Destruir si no tiene el script
            }
             yield return new WaitForSeconds(0.2f); // Pequeño delay entre spawns
        }

        // 3. Esperar a que se derroten suficientes Maggies
        // La notificación viene de MaggieController a través de ReportMaggieDefeated()
        // El estado del Controller sigue siendo Attacking_Shielding
    }

    // Llamado por MaggieController cuando una muere
    public void ReportMaggieDefeated(MaggieController defeatedMaggie)
    {
        if (activeMaggies.Contains(defeatedMaggie))
        {
            activeMaggies.Remove(defeatedMaggie);
            maggiesDefeatedCount++;

            // Comprobar si se cumplió la condición
            if (maggiesDefeatedCount >= maggiesToDefeat && controller.CurrentState == MargaretController.BossState.Attacking_Shielding)
            {
                // Condición cumplida, salir del escudo
                StopAllCoroutines(); // Detiene la corutina de spawn si aún estaba activa
                StartCoroutine(EndShieldSequenceCoroutine());
            }
        }
    }

    private IEnumerator EndShieldSequenceCoroutine()
    {
         if (controller.CurrentState == MargaretController.BossState.Dead)
         {
              health.SetInvulnerable(false);
              yield break;
         }

        animator?.SetTrigger("ShieldEndTrigger");
        // TODO: Play Shield End SFX/VFX

         // Destruir Maggies restantes (opcional)
         foreach (MaggieController maggie in activeMaggies)
         {
             if (maggie != null) Destroy(maggie.gameObject); // O devuélvelos al pool
         }
         activeMaggies.Clear();


        yield return new WaitForSeconds(0.5f); // Tiempo para que baje el escudo

        health.SetInvulnerable(false); // MUY IMPORTANTE

        // Decidir qué hacer después (¿Swoop inmediato? ¿Volver a Idle?)
        // Ejemplo: Swoop inmediato
        // controller.StartSwoopAttack(); // Necesita que StartSwoopAttack no chequee el estado Idle

        // Ejemplo: Volver a Idle
        controller.OnAttackComplete(false);
    }

     // Limpieza por si Margaret muere durante el escudo
    void OnDestroy() {
        foreach (MaggieController maggie in activeMaggies) {
             if (maggie != null) Destroy(maggie.gameObject);
        }
    }
}