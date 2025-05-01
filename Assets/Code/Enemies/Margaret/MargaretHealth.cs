using UnityEngine;
using System;

[RequireComponent(typeof(Healt))] // Asegurarse de que Healt esté presente
public class MargaretHealth : MonoBehaviour
{
    [Header("Phase Thresholds (Based on Healt.maxHealth)")]
    [SerializeField] [Range(0f, 1f)] private float phase2ThresholdPercent = 0.7f;
    [SerializeField] [Range(0f, 1f)] private float phase3ThresholdPercent = 0.3f;

    [Header("Margaret State")]
    [SerializeField] private bool isPhaseInvulnerable = false; // Para el escudo y mecánicas de fase

    // Propiedades públicas de solo lectura para estado
    public int CurrentPhase { get; private set; } = 1;
    public bool IsPhaseInvulnerable => isPhaseInvulnerable;
    public bool IsEffectivelyDead => isDead; // Para que otros scripts sepan si ya procesamos la muerte

    // --- Variables privadas de estado ---
    private Healt healtComponent;
    private int phase2HealthThresholdInt; // Umbral como entero
    private int phase3HealthThresholdInt; // Umbral como entero
    private int lastKnownHealth;        // Para detectar cambios
    private bool phase2Reached = false;
    private bool phase3Reached = false;
    private bool isDead = false;         // Estado interno de muerte

    // --- Eventos propios para comunicar hacia afuera (Controller) ---
    public event Action<int> OnPhaseChanged; // Envía la nueva fase
    public event Action OnDeath;          // Notifica la muerte de Margaret
    // El evento OnHealthChanged es redundante si otros scripts pueden leer Healt.currentHealth

    void Awake()
    {
        healtComponent = GetComponent<Healt>();

        if (healtComponent == null)
        {
            Debug.LogError($"MargaretHealth: No se encontró el componente Healt en {gameObject.name}. ¡Es necesario!", this);
            this.enabled = false;
            return;
        }

        // Calcular umbrales INT basados en el maxHealth INT de Healt.cs
        // Asegúrate de que healtComponent.maxHealth tenga un valor correcto al iniciar Awake.
        // Si maxHealth se establece en Healt.Start(), esto podría dar 0 aquí.
        // Es más seguro calcularlos en Start() o asumir que maxHealth está listo en Awake.
        if (healtComponent.maxHealt <= 0)
        {
             Debug.LogWarning($"Healt.maxHealth es 0 o negativo en Awake para {gameObject.name}. Los umbrales de fase pueden ser incorrectos. Asegúrate de que se inicialice antes.", healtComponent);
             // Podríamos intentar recalcular en Start o asumir un valor por defecto
        }
        CalculateIntegerThresholds();


        // Guardar la vida inicial para la primera comprobación
        lastKnownHealth = healtComponent.currentHealt;

        // Resetear estado al inicio
        ResetStateInternals();
    }

     void Start()
    {
        // Si maxHealth no estaba listo en Awake, intentar recalcular umbrales aquí
         if (phase2HealthThresholdInt == 0 && healtComponent.maxHealt > 0) {
              Debug.LogWarning("Recalculando umbrales de fase en Start porque maxHealth no estaba listo en Awake.");
              CalculateIntegerThresholds();
         }

        // Guardar vida inicial (de nuevo por si cambió entre Awake y Start)
         lastKnownHealth = healtComponent.currentHealt;

         // Comprobar estado inicial
         CheckState();
    }


    // Usar LateUpdate para comprobar el estado DESPUÉS de que Healt.cs haya procesado el daño en Update/FixedUpdate
    void LateUpdate()
    {
        if (healtComponent == null || isDead) return; // Si no hay Healt o ya está muerto, no hacer nada

        CheckState();
    }

    void CheckState()
    {
        int currentHealth = healtComponent.currentHealt;

        // --- 1. Chequear Muerte (ANTES que nada) ---
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Debug.Log("MargaretHealth detectó muerte (Healt <= 0) en LateUpdate.");
            OnDeath?.Invoke(); // Notificar al Controller INMEDIATAMENTE
            // No podemos detener Healt.Die(), pero notificamos antes si es posible.
            return; // Salir para no procesar cambios de vida/fase post-mortem
        }

        // --- 2. Detectar Cambio de Vida ---
        if (currentHealth != lastKnownHealth)
        {
            int healthChange = currentHealth - lastKnownHealth; // Negativo si es daño, positivo si es cura

            // --- 3. Simular Invulnerabilidad (Heal Back) ---
            if (isPhaseInvulnerable && healthChange < 0) // Si es invulnerable y recibió daño
            {
                int damageTaken = -healthChange; // El daño que Healt aplicó
                Debug.LogWarning($"Margaret era invulnerable pero recibió {damageTaken} daño. Curando de vuelta.");
                healtComponent.Heal(damageTaken); // Intentar curar inmediatamente
                // ¡OJO! Esto puede causar un bucle si Heal() también llama a Update/LateUpdate
                // y se detecta el cambio de nuevo. Es una limitación del enfoque.
                // Actualizamos currentHealth para evitar procesar el daño en el chequeo de fase.
                currentHealth = healtComponent.currentHealt; // Releer la vida después de curar
            }

            // --- 4. Chequear Transición de Fase (solo si no se curó de vuelta o si fue curación normal) ---
             // Solo chequear fases si la vida realmente cambió y no estamos muertos
             if (currentHealth != lastKnownHealth && !isDead)
             {
                  CheckPhaseTransition(currentHealth);
             }

            // Actualizar la última vida conocida
            lastKnownHealth = currentHealth;
        }
    }


    // --- Calcular Umbrales Enteros ---
    void CalculateIntegerThresholds()
{
    phase2HealthThresholdInt = Mathf.FloorToInt(healtComponent.maxHealt * phase2ThresholdPercent); // 70% de 250 = 175
    phase3HealthThresholdInt = Mathf.FloorToInt(healtComponent.maxHealt * phase3ThresholdPercent); // 30% de 250 = 75

    // Debug para verificar los umbrales
    Debug.Log($"Umbrales calculados: Fase 2 (≤{phase2HealthThresholdInt}), Fase 3 (≤{phase3HealthThresholdInt}) | MaxHealth: {healtComponent.maxHealt}");
}


    // --- Chequear transición de fase basado en la vida entera actual ---
    private void CheckPhaseTransition(int currentHealthValue)
{
    Debug.Log($"Chequeando fase. Vida actual: {currentHealthValue}");

    // Fase 2 (≤70% = 175)
    if (!phase2Reached && CurrentPhase == 1 && currentHealthValue <= phase2HealthThresholdInt)
    {
        CurrentPhase = 2;
        phase2Reached = true;
        OnPhaseChanged?.Invoke(CurrentPhase);
        Debug.Log($"Margaret entró en FASE 2 (Vida: {currentHealthValue} ≤ {phase2HealthThresholdInt})");
    }
    // Fase 3 (≤30% = 75)
    else if (!phase3Reached && CurrentPhase == 2 && currentHealthValue <= phase3HealthThresholdInt)
    {
        CurrentPhase = 3;
        phase3Reached = true;
        OnPhaseChanged?.Invoke(CurrentPhase);
        Debug.Log($"Margaret entró en FASE 3 (Vida: {currentHealthValue} ≤ {phase3HealthThresholdInt})");
    }
}

    // --- Controlar la invulnerabilidad específica de FASE ---
    public void SetPhaseInvulnerable(bool status)
    {
         if (isDead) return; // No cambiar si ya está muerto
        isPhaseInvulnerable = status;
         // Debug.Log($"Margaret Phase Invulnerability set to: {status}");
    }

    // --- Resetear el estado para reiniciar la pelea ---
    public void ResetState() // Renombrado para claridad
    {
        if (healtComponent == null) return;

        // Resetear Healt directamente (ya que no tiene método Reset)
        healtComponent.currentHealt = healtComponent.maxHealt;
        // No podemos resetear healtComponent.isImmune o corutinas directamente

        ResetStateInternals();

        // Forzar chequeo inicial
        CheckState();
         Debug.Log("MargaretHealth state reset.");
    }

    // --- Resetea solo las variables internas de este script ---
    private void ResetStateInternals()
    {
         CurrentPhase = 1;
        phase2Reached = false;
        phase3Reached = false;
        isDead = false;
        isPhaseInvulnerable = false;
        // Asegurarse de tener los umbrales correctos
         if (healtComponent != null && healtComponent.maxHealt > 0) {
              CalculateIntegerThresholds();
              lastKnownHealth = healtComponent.currentHealt; // Actualizar vida conocida
         } else if(healtComponent != null) {
             lastKnownHealth = healtComponent.maxHealt; // Asumir max si no se puede leer actual
         }
    }
}