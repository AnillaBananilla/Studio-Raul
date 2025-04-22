using UnityEngine;
using System.Collections;

public class MargaretAttack_Distance : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MargaretController controller;
    [SerializeField] private Transform firePoint; // De donde salen los proyectiles
    [SerializeField] private GameObject projectilePrefab; // Prefab del proyectil guiado

    [Header("Attack Settings")]
    [SerializeField] private int shotsInBurst = 3;
    [SerializeField] private float delayBetweenShots = 0.3f;
    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private float projectileTurnSpeed = 5f; // Para homing suave
    [SerializeField] private float timeBetweenVolleys = 0.5f; // Tiempo antes de la siguiente ráfaga (si hay combo)

    private int currentMaxCombo;
    private bool moveBetweenVolleys;

    // Llamado por el Controller para iniciar la secuencia completa
     public void StartAttackSequence(int currentPhase, int maxCombo, bool moveBetween)
    {
        this.currentMaxCombo = maxCombo;
        this.moveBetweenVolleys = moveBetween;
        StartSingleVolley(currentPhase); // Inicia la primera ráfaga
    }

     // Inicia una sola ráfaga de 3 proyectiles
    public void StartSingleVolley(int currentPhase)
    {
         if (controller.CurrentState == MargaretController.BossState.Dead) return;
        StartCoroutine(AttackCoroutine(currentPhase));
    }


    private IEnumerator AttackCoroutine(int currentPhase)
    {
        // animator?.SetTrigger("AttackDistanceTrigger"); // O manejar desde Controller

        for (int i = 0; i < shotsInBurst; i++)
        {
            if (controller.CurrentState == MargaretController.BossState.Dead) yield break; // Salir si muere

            // TODO: Play Fire SFX

            // Instanciar proyectil (USA POOLING EN UN JUEGO REAL)
            GameObject projGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            ProjectileMAG guidedProjectile = projGO.GetComponent<ProjectileMAG>(); // Asume que tienes un script Projectile

            if (guidedProjectile != null)
            {
                // Configurar el proyectil
                guidedProjectile.Initialize(controller.GetPlayerTransform(), projectileSpeed, projectileTurnSpeed); // Pasa el target y parámetros
            } else {
                 Debug.LogWarning("Projectile prefab missing Projectile script!");
                 // O solo darle velocidad si no es guiado
                 Rigidbody2D rb = projGO.GetComponent<Rigidbody2D>();
                 if(rb != null)
                 {
                     Vector2 direction = (controller.GetPlayerTransform().position - firePoint.position).normalized;
                     rb.velocity = direction * projectileSpeed;
                 }
            }


            yield return new WaitForSeconds(delayBetweenShots);
        }

        // Esperar un poco antes de decidir si hay otra ráfaga o terminar
        yield return new WaitForSeconds(timeBetweenVolleys);

        // Notificar al controller que esta ráfaga terminó
        controller.OnDistanceAttackVolleyComplete(currentMaxCombo, moveBetweenVolleys);
    }
}