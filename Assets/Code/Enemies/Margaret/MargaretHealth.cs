using UnityEngine;
using System; // Necesario para Action

public class MargaretHealth : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHealth = 1000f;
    [SerializeField] [Range(0f, 1f)] private float phase2ThresholdPercent = 0.7f;
    [SerializeField] [Range(0f, 1f)] private float phase3ThresholdPercent = 0.3f;

    [Header("State")]
    [SerializeField] private bool isInvulnerable = false; // Para el escudo
    public float CurrentHealth { get; private set; }
    public int CurrentPhase { get; private set; } = 1;

    private float phase2HealthThreshold;
    private float phase3HealthThreshold;
    private bool phase2Reached = false;
    private bool phase3Reached = false;
    private bool isDead = false;

    // Eventos para comunicar cambios al Controller u otros sistemas
    public event Action<int> OnPhaseChanged; // Envía la nueva fase
    public event Action OnDeath;
    public event Action<float> OnHealthChanged; // Envía el porcentaje de vida actual

    void Awake()
    {
        CurrentHealth = maxHealth;
        phase2HealthThreshold = maxHealth * phase2ThresholdPercent;
        phase3HealthThreshold = maxHealth * phase3ThresholdPercent;
    }

    public void TakeDamage(float amount)
    {
        if (isInvulnerable || isDead) return;

        CurrentHealth -= amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(CurrentHealth / maxHealth);
        // Debug.Log($"Margaret Health: {CurrentHealth}/{maxHealth}");

        // TODO: Play Hurt Animation/Sound

        CheckPhaseTransition();

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void CheckPhaseTransition()
    {
        if (!phase3Reached && CurrentPhase == 2 && CurrentHealth <= phase3HealthThreshold)
        {
            CurrentPhase = 3;
            phase3Reached = true;
            OnPhaseChanged?.Invoke(CurrentPhase);
            // Debug.Log("Entered Phase 3");
        }
        else if (!phase2Reached && CurrentPhase == 1 && CurrentHealth <= phase2HealthThreshold)
        {
            CurrentPhase = 2;
            phase2Reached = true;
            OnPhaseChanged?.Invoke(CurrentPhase);
            // Debug.Log("Entered Phase 2");
        }
    }

    public void SetInvulnerable(bool status)
    {
        isInvulnerable = status;
        // Debug.Log($"Margaret Invulnerable: {status}");
    }

    private void Die()
    {
        if(isDead) return;

        isDead = true;
        // Debug.Log("Margaret Died");
        OnDeath?.Invoke();
        // TODO: Trigger Death Animation/Effects
        // Podrías desactivar colliders aquí también
    }

    // --- Opcional: Para pruebas o mecánicas de curación ---
    public void Heal(float amount)
    {
        if (isDead) return;
        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth / maxHealth);
        CheckPhaseTransition(); // Re-chequear por si alguna mecánica rara la baja y sube de fase
    }

    public void ResetHealth() // Para reiniciar la pelea
    {
         CurrentHealth = maxHealth;
         CurrentPhase = 1;
         phase2Reached = false;
         phase3Reached = false;
         isDead = false;
         isInvulnerable = false;
         OnHealthChanged?.Invoke(1f);
    }
}