using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Para listas y diccionarios
using System; // Para Action

public class MargaretController : MonoBehaviour
{
    public enum BossState { Idle, FlyingToPosition, Attacking_Distance, Attacking_Jump, Attacking_Shielding, Attacking_Swoop, Attacking_BulletHell_Charging, Attacking_BulletHell_Firing, Stunned, Transitioning, Dead }

    [Header("Core Components")]
    [SerializeField] private MargaretHealth margaretHealth;
    [SerializeField] private MargaretMovement margaretMovement;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource; // O un manager de audio

    [Header("Targeting")]
    [SerializeField] private Transform playerTransform;

    [Header("Attack Scripts")]
    [SerializeField] private MargaretAttack_Distance distanceAttack;
    [SerializeField] private MargaretAttack_Jump jumpAttack;
    [SerializeField] private MargaretAttack_ShieldMaggies shieldAttack;
    [SerializeField] private MargaretAttack_Swoop swoopAttack;
    [SerializeField] private MargaretAttack_BulletHell bulletHellAttack;

    [Header("Behavior Timing")]
    [SerializeField] private float minIdleTime = 1.5f;
    [SerializeField] private float maxIdleTime = 3.0f;
    [SerializeField] private float phaseTransitionTime = 2.0f; // Tiempo para efecto de transición
    [SerializeField] private float stunDuration = 1.5f; // Tiempo quieta después de ciertos ataques

    [Header("Attack Cooldowns")]
    [SerializeField] private float shieldCooldown = 20f;
    [SerializeField] private float bulletHellCooldown = 30f;
    [SerializeField] private float swoopCooldown = 10f; // Cooldown para el Swoop si no es inmediato

    [Header("Movement & Positioning")]
    [SerializeField] private List<Transform> flightPlatforms; // Puntos donde puede posarse
    [SerializeField] private Transform centerStagePoint; // Referencia central

    // --- State Machine ---
    public BossState CurrentState { get; private set; } = BossState.Idle;
    private float idleTimer;
    private float shieldCooldownTimer;
    private float bulletHellCooldownTimer;
    private float swoopCooldownTimer;
    private int currentDistanceComboCount = 0;
    private Transform currentPlatform = null;


    void Start()
    {
        if (playerTransform == null)
        {
            // Intenta encontrar al jugador por Tag si no está asignado
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerTransform = playerObj.transform;
            else Debug.LogError("MargaretController: Player Transform not found!");
        }

        // Suscribirse a eventos de vida
        margaretHealth.OnPhaseChanged += HandlePhaseChange;
        margaretHealth.OnDeath += HandleDeath;
        margaretHealth.OnHealthChanged += (healthPercent) => { /* Puede usarse para UI */ };

        ResetTimers();
        SetState(BossState.Idle); // Empezar en Idle o tal vez FlyingToPosition
    }

    void Update()
    {
        if (CurrentState == BossState.Dead) return;

        // Actualizar Cooldowns
        shieldCooldownTimer -= Time.deltaTime;
        bulletHellCooldownTimer -= Time.deltaTime;
        swoopCooldownTimer -= Time.deltaTime;

        // --- FSM (Finite State Machine) ---
        switch (CurrentState)
        {
            case BossState.Idle:
                // animator?.SetBool("IsIdle", true); // Asegura estado de animación
                idleTimer -= Time.deltaTime;
                if (idleTimer <= 0 && !margaretMovement.IsMoving())
                {
                    // animator?.SetBool("IsIdle", false);
                    ChooseNextAction();
                }
                break;

            case BossState.FlyingToPosition:
                // La lógica de llegada la maneja la corutina en MargaretMovement
                // Cuando llega, el callback (si se definió) pondrá el estado a Idle
                break;

            case BossState.Attacking_Distance:
            case BossState.Attacking_Jump:
            case BossState.Attacking_Shielding:
            case BossState.Attacking_Swoop:
            case BossState.Attacking_BulletHell_Charging:
            case BossState.Attacking_BulletHell_Firing:
                // La lógica está dentro de los scripts de ataque y sus corutinas
                // Esperando a que llamen a OnAttackComplete() o OnAttackPhaseEnd()
                break;

            case BossState.Stunned:
                // Espera un tiempo y vuelve a Idle
                // La duración del stun se maneja en la corutina que lo inicia
                break;

            case BossState.Transitioning:
                // Espera a que termine el tiempo de transición
                 // La duración se maneja en la corutina que lo inicia
                break;

            case BossState.Dead:
                // No hacer nada más
                break;
        }
    }

    private void ChooseNextAction()
    {
        if (CurrentState != BossState.Idle) return; // Solo decidir desde Idle

        // --- Lógica de Selección de Ataque ---
        // Esta es la parte clave de la IA. Aquí decides qué hacer basado en fase, cooldowns, etc.
        // Ejemplo simple y placeholder:

        int phase = margaretHealth.CurrentPhase;
        List<Action> possibleActions = new List<Action>();

        // Acción básica: Volar a otra plataforma
        possibleActions.Add(FlyToRandomPlatform);

        // Ataque a Distancia (siempre posible, quizás con diferente frecuencia)
        possibleActions.Add(StartDistanceAttack);

        // Salto Pisotón (siempre posible?)
        possibleActions.Add(StartJumpAttack);


        // Ataques de Fase 2+
        if (phase >= 2)
        {
            if (shieldCooldownTimer <= 0) possibleActions.Add(StartShieldAttack);
            if (swoopCooldownTimer <= 0) possibleActions.Add(StartSwoopAttack); // Añadir Swoop en Fase 2
        }

        // Ataques de Fase 3+
        if (phase >= 3)
        {
            if (bulletHellCooldownTimer <= 0) possibleActions.Add(StartBulletHellAttack);
        }


        // --- Seleccionar Acción ---
        if (possibleActions.Count > 0)
        {
            // Selecciona una acción aleatoria (puedes hacerla más inteligente con pesos)
            int randomIndex = UnityEngine.Random.Range(0, possibleActions.Count);
            possibleActions[randomIndex].Invoke(); // Ejecuta la acción seleccionada
        }
        else
        {
            // Si no hay nada más, vuela a algún sitio
            FlyToRandomPlatform();
        }
    }

    // --- Métodos para Iniciar Acciones/Ataques ---

    private void FlyToRandomPlatform()
    {
        if (flightPlatforms == null || flightPlatforms.Count == 0)
        {
            Debug.LogWarning("MargaretController: No flight platforms defined!");
            SetState(BossState.Idle); // Volver a idle si no hay donde ir
            return;
        }

        Transform nextPlatform = flightPlatforms[UnityEngine.Random.Range(0, flightPlatforms.Count)];
        if (nextPlatform == currentPlatform && flightPlatforms.Count > 1) // Evita quedarse en la misma si hay más
        {
            nextPlatform = flightPlatforms[(flightPlatforms.IndexOf(currentPlatform) + 1) % flightPlatforms.Count];
        }
        currentPlatform = nextPlatform;

        SetState(BossState.FlyingToPosition);
        margaretMovement.StartFlyToTarget(currentPlatform.position, () => {
            SetState(BossState.Idle); // Cuando llega, vuelve a Idle
        });
    }


    private void StartDistanceAttack()
    {
        SetState(BossState.Attacking_Distance);
        // Decide si es un combo basado en la fase
        bool isCombo = margaretHealth.CurrentPhase >= 2;
        int maxCombo = isCombo ? UnityEngine.Random.Range(1, 5) : 1; // 1 a 4 disparos en fase 2/3
        bool moveBetween = isCombo; // Moverse entre ráfagas en fase 2/3

        currentDistanceComboCount = 0; // Resetear contador de combo
        distanceAttack.StartAttackSequence(margaretHealth.CurrentPhase, maxCombo, moveBetween);
    }

    private void StartJumpAttack()
    {
        SetState(BossState.Attacking_Jump);
        int jumpCount = UnityEngine.Random.Range(2, 5); // 2 a 4 saltos
        jumpAttack.StartAttackSequence(jumpCount);
    }

    private void StartShieldAttack()
    {
        SetState(BossState.Attacking_Shielding);
        shieldCooldownTimer = shieldCooldown; // Reinicia cooldown
        shieldAttack.StartAttack();
    }

     private void StartSwoopAttack()
    {
        SetState(BossState.Attacking_Swoop);
        swoopCooldownTimer = swoopCooldown; // Reinicia cooldown
        // Determina puntos de inicio y fin del swoop (ej. fuera de cámara)
        Vector2 startPos = GetOffScreenPosition();
        Vector2 endPos = GetOffScreenPosition(oppositeSide: true); // Pasar por el centro o jugador
        swoopAttack.StartAttack(startPos, endPos);
    }

    private void StartBulletHellAttack()
    {
        SetState(BossState.Attacking_BulletHell_Charging);
        bulletHellCooldownTimer = bulletHellCooldown; // Reinicia cooldown
        bulletHellAttack.StartAttack();
    }

    // --- Callbacks y Manejadores de Estado ---

    public void OnAttackComplete(bool enterStun = false)
    {
        if(CurrentState == BossState.Dead) return;

        if (enterStun)
        {
            StartCoroutine(StunnedCoroutine());
        }
        else
        {
            SetState(BossState.Idle);
        }
    }

     public void OnDistanceAttackVolleyComplete(int maxCombo, bool moveBetween)
    {
         if(CurrentState == BossState.Dead) return;

        currentDistanceComboCount++;
        if (margaretHealth.CurrentPhase > 1 && currentDistanceComboCount < maxCombo)
        {
             // Concatenar siguiente ráfaga
             if (moveBetween)
             {
                 // Volar rápido a una posición cercana ANTES de la siguiente ráfaga
                 Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * 3f; // Ajusta el radio
                 Vector2 nextPos = (Vector2)transform.position + randomOffset;
                 SetState(BossState.FlyingToPosition); // Estado temporal mientras se mueve
                 margaretMovement.StartFlyToTarget(nextPos, () => {
                     // Cuando llega, inicia la siguiente ráfaga
                     SetState(BossState.Attacking_Distance);
                     distanceAttack.StartSingleVolley(margaretHealth.CurrentPhase);
                 });
             }
             else
             {
                 // Dispara de nuevo inmediatamente
                 distanceAttack.StartSingleVolley(margaretHealth.CurrentPhase);
             }
        }
        else
        {
            // Terminó la secuencia de ráfagas
             OnAttackComplete(false); // Volver a Idle sin stun
        }
    }


    public void OnBulletHellChargeComplete()
    {
        if(CurrentState == BossState.Dead) return;
        SetState(BossState.Attacking_BulletHell_Firing);
        // El script de ataque bullet hell ahora dispara
    }

    private void HandlePhaseChange(int newPhase)
    {
        if(CurrentState == BossState.Dead) return;

        Debug.Log($"Margaret Entering Phase: {newPhase}");
        StartCoroutine(PhaseTransitionCoroutine(newPhase));
        // TODO: Trigger Phase Change Dialogue/Effects
    }

    private IEnumerator PhaseTransitionCoroutine(int newPhase)
    {
        SetState(BossState.Transitioning);
        margaretMovement.StopCurrentMovement(); // Detiene movimiento si lo había
        margaretHealth.SetInvulnerable(true); // Invulnerable durante transición
        animator?.SetTrigger("PhaseChangeTrigger"); // Animación opcional
        // TODO: Play Phase Transition VFX/SFX

        yield return new WaitForSeconds(phaseTransitionTime);

        margaretHealth.SetInvulnerable(false); // Quita invulnerabilidad
        ResetCooldownsForPhase(newPhase); // Ajusta cooldowns si es necesario
        SetState(BossState.Idle); // Volver a Idle después de la transición
    }


    private IEnumerator StunnedCoroutine()
    {
        SetState(BossState.Stunned);
        // animator?.SetTrigger("StunnedTrigger"); // Animación opcional
        margaretMovement.StopCurrentMovement();
        // TODO: Play Stunned VFX/SFX (ej. Recuperando energía)

        yield return new WaitForSeconds(stunDuration);

        if(CurrentState != BossState.Dead) // Check por si muere mientras está stunned
          SetState(BossState.Idle);
    }


    private void HandleDeath()
    {
        SetState(BossState.Dead);
        margaretMovement.StopCurrentMovement();
        StopAllCoroutines(); // Detiene cualquier acción pendiente
        // TODO: Desactivar Colliders, iniciar animación de muerte, efectos finales, llamar a cinemática final...
        this.enabled = false; // Desactiva este script
    }

    public void SetState(BossState newState)
    {
        if (CurrentState == BossState.Dead && newState != BossState.Dead) return; // No salir de muerto

        //Debug.Log($"Margaret State: {CurrentState} -> {newState}");
        CurrentState = newState;

        // Podrías resetear flags de animación aquí si usas bools
        // animator?.SetBool("IsIdle", newState == BossState.Idle);
        // animator?.SetBool("IsFlying", newState == BossState.FlyingToPosition);
        // ...etc

        if (newState == BossState.Idle)
        {
            ResetIdleTimer();
        }
    }

    private void ResetIdleTimer()
    {
        float baseIdle = UnityEngine.Random.Range(minIdleTime, maxIdleTime);
        // Reducir tiempo idle en fases avanzadas
        if (margaretHealth.CurrentPhase == 2) baseIdle *= 0.75f;
        if (margaretHealth.CurrentPhase == 3) baseIdle *= 0.5f;
        idleTimer = baseIdle;
    }

    private void ResetTimers()
    {
        shieldCooldownTimer = 0; // Empezar disponible? O poner un cooldown inicial
        bulletHellCooldownTimer = 5f; // Cooldown inicial
        swoopCooldownTimer = 0;
        ResetIdleTimer();
    }

     private void ResetCooldownsForPhase(int phase)
    {
        // Opcional: Podrías querer resetear o reducir cooldowns al cambiar de fase
        if(phase == 2)
        {
            shieldCooldownTimer = shieldCooldown * 0.5f; // Quizás el escudo esté listo antes en fase 2
        }
         if(phase == 3)
        {
             bulletHellCooldownTimer = bulletHellCooldown * 0.5f; // Bullet hell listo antes
        }
    }

    // --- Helper para Swoop ---
    private Vector2 GetOffScreenPosition(bool oppositeSide = false)
    {
        // Obtén los límites de la cámara principal
        Camera mainCam = Camera.main;
        float screenX = Screen.width;
        float screenY = Screen.height;

        // Elige un lado (arriba, abajo, izq, der)
        int side = UnityEngine.Random.Range(0, 4);
        if (oppositeSide && CurrentState == BossState.Attacking_Swoop)
        {
             // Si está haciendo swoop, intenta salir por el lado opuesto al que entró
             // (Requiere guardar por dónde entró) - Simplificado por ahora
        }

        Vector3 edgePoint = Vector3.zero;
        switch (side)
        {
            case 0: edgePoint = mainCam.ScreenToWorldPoint(new Vector3(UnityEngine.Random.Range(0, screenX), screenY, mainCam.nearClipPlane + 10)); break; // Arriba
            case 1: edgePoint = mainCam.ScreenToWorldPoint(new Vector3(UnityEngine.Random.Range(0, screenX), 0, mainCam.nearClipPlane + 10)); break; // Abajo
            case 2: edgePoint = mainCam.ScreenToWorldPoint(new Vector3(0, UnityEngine.Random.Range(0, screenY), mainCam.nearClipPlane + 10)); break; // Izquierda
            case 3: edgePoint = mainCam.ScreenToWorldPoint(new Vector3(screenX, UnityEngine.Random.Range(0, screenY), mainCam.nearClipPlane + 10)); break; // Derecha
        }

        // Añade un offset para asegurar que esté fuera de vista
        Vector2 directionFromCenter = ((Vector2)edgePoint - (Vector2)centerStagePoint.position).normalized;
        return (Vector2)edgePoint + directionFromCenter * 2f; // Ajusta el offset
    }

     // --- Referencia para otros scripts ---
    public Transform GetPlayerTransform() => playerTransform;
    public Vector2 GetTargetPlatformPosition() => currentPlatform != null ? currentPlatform.position : (Vector2)transform.position;


     // --- Interacción con diálogo ---
    public void TriggerDialogue(string dialogueID)
    {
        // Llama a tu sistema de diálogo
        // DialogueManager.Instance?.ShowDialogue(dialogueID);
        Debug.Log($"DIALOGUE TRIGGER: {dialogueID}");
    }
}