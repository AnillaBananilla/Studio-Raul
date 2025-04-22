using UnityEngine;

public class MaggieController : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] private GameObject deathVFX;

    private float currentHealth;
    private MargaretAttack_ShieldMaggies shieldAttackScript; // Referencia para notificar

    void Awake()
    {
        currentHealth = maxHealth;
    }

    // Llamado por MargaretAttack_ShieldMaggies al instanciar
    public void Initialize(MargaretAttack_ShieldMaggies ownerScript)
    {
        this.shieldAttackScript = ownerScript;
        currentHealth = maxHealth; // Resetear vida si viene de un pool
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        // TODO: Play Hurt SFX/VFX?

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (shieldAttackScript != null)
        {
            shieldAttackScript.ReportMaggieDefeated(this); // Notifica al script de Margaret
        }

        if (deathVFX != null) Instantiate(deathVFX, transform.position, Quaternion.identity);
        // TODO: Play Death SFX

        Destroy(gameObject); // O: gameObject.SetActive(false); PoolManager.Instance.ReturnMaggie(gameObject);
    }

     // Ejemplo simple de detección de ataque del jugador
     void OnTriggerEnter2D(Collider2D other) {
         if(other.CompareTag("PlayerAttack")) // Asume que los ataques del jugador tienen este tag
         {
             // Obtener daño del ataque del jugador (necesitarías un script en el ataque)
             // float damage = other.GetComponent<PlayerAttack>().damageAmount;
             float damage = 10f; // Placeholder
             TakeDamage(damage);
         }
     }
}