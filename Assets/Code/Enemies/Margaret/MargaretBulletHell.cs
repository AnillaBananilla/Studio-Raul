using UnityEngine;
using System.Collections;

public class MargaretAttack_BulletHell : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MargaretController controller;
    [SerializeField] private MargaretHealth health;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletSpawnCenter; // Punto central de donde salen los proyectiles
    [SerializeField] private GameObject bulletPrefab; // Prefab del proyectil (NO guiado)

    [Header("Attack Settings")]
    [SerializeField] private float chargeTime = 2.5f;
    [SerializeField] private int numberOfWaves = 5; // Cuántas oleadas de proyectiles
    [SerializeField] private int bulletsPerWave = 20; // Cuántos proyectiles por oleada
    [SerializeField] private float timeBetweenWaves = 0.3f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float waveAngleOffset = 15f; // Rotar cada oleada un poco

    [Header("Charge Effect (Optional)")]
    [SerializeField] private GameObject chargeVFXPrefab;
    [SerializeField] private float pushbackRadius = 2f; // Radio para repeler al jugador
    [SerializeField] private float pushbackForce = 50f;

    private GameObject currentChargeVFX;

    public void StartAttack()
    {
         if (controller.CurrentState == MargaretController.BossState.Dead) return;
        StartCoroutine(BulletHellSequenceCoroutine());
    }

    private IEnumerator BulletHellSequenceCoroutine()
    {
        // 1. Fase de Carga
        health.SetInvulnerable(true); // Invulnerable mientras carga
        animator?.SetTrigger("BulletHellChargeTrigger");
        // TODO: Play Charge Start SFX

        if (chargeVFXPrefab != null)
            currentChargeVFX = Instantiate(chargeVFXPrefab, bulletSpawnCenter.position, Quaternion.identity, bulletSpawnCenter);

        float chargeTimer = 0f;
        while (chargeTimer < chargeTime)
        {
            if (controller.CurrentState == MargaretController.BossState.Dead)
            {
                 if (currentChargeVFX != null) Destroy(currentChargeVFX);
                 health.SetInvulnerable(false);
                 yield break;
            }

             chargeTimer += Time.deltaTime;
             // TODO: Animar VFX de carga (intensidad, tamaño)

             // Repeler al jugador si se acerca (Opcional)
             Collider2D[] hits = Physics2D.OverlapCircleAll(bulletSpawnCenter.position, pushbackRadius);
             foreach(Collider2D hit in hits)
             {
                 if (hit.CompareTag("Player"))
                 {
                     Rigidbody2D playerRb = hit.GetComponent<Rigidbody2D>();
                     if(playerRb != null)
                     {
                         Vector2 direction = (hit.transform.position - bulletSpawnCenter.position).normalized;
                         playerRb.AddForce(direction * pushbackForce * Time.deltaTime, ForceMode2D.Impulse);
                     }
                 }
             }
             yield return null;
        }

        if (currentChargeVFX != null) Destroy(currentChargeVFX);
        // TODO: Play Charge End / Firing Start SFX

        // Notificar al controller que la carga terminó y empieza el disparo
        controller.OnBulletHellChargeComplete();
        animator?.SetTrigger("BulletHellFireTrigger");

        // 2. Fase de Disparo
        float currentAngleOffset = 0f;
        for (int wave = 0; wave < numberOfWaves; wave++)
        {
             if (controller.CurrentState == MargaretController.BossState.Dead)
             {
                 health.SetInvulnerable(false); // Asegurar quitar invulnerabilidad
                 yield break;
             }

            float angleStep = 360f / bulletsPerWave;
            for (int i = 0; i < bulletsPerWave; i++)
            {
                float angle = (i * angleStep + currentAngleOffset) * Mathf.Deg2Rad;
                Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                // Instanciar Proyectil (USA POOLING!)
                GameObject bulletGO = Instantiate(bulletPrefab, bulletSpawnCenter.position, Quaternion.identity);
                Rigidbody2D rb = bulletGO.GetComponent<Rigidbody2D>();
                 ProjectileMAG projScript = bulletGO.GetComponent<ProjectileMAG>(); // Asumiendo script Projectile básico

                if(rb != null)
                {
                    rb.velocity = direction * bulletSpeed;
                }
                if (projScript != null)
                {
                     // Inicializar si es necesario (ej. daño)
                     // projScript.Initialize(bulletDamage);
                }
            }
            currentAngleOffset += waveAngleOffset; // Rota la siguiente oleada
             // TODO: Play Wave Fire SFX (opcional)
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        // 3. Terminar
        health.SetInvulnerable(false); // Quita invulnerabilidad
        controller.OnAttackComplete(true); // Entrar en Stun después del ataque
    }
}