using UnityEngine;

public class AoEDamage : MonoBehaviour
{
    [SerializeField] private float maxDamage = 50f; // Daño justo en el centro
    [SerializeField] private float minDamagePercent = 0.2f; // Porcentaje de daño en el borde exterior
    [SerializeField] private float damageRadius = 3f; // Radio del área de efecto
    [SerializeField] private float lifeTime = 0.2f; // Cuánto tiempo está activa el área

    private CircleCollider2D damageCollider;
    private Vector2 centerPoint;

    void Awake()
    {
        damageCollider = GetComponent<CircleCollider2D>();
        if (damageCollider == null)
        {
             Debug.LogError("AoEDamage needs a CircleCollider2D component!");
             damageCollider = gameObject.AddComponent<CircleCollider2D>();
        }
        damageCollider.isTrigger = true;
        damageCollider.radius = damageRadius;
        centerPoint = transform.position;
        Destroy(gameObject, lifeTime); // Autodestrucción
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            /*PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Calcular daño basado en la distancia
                float distance = Vector2.Distance(centerPoint, other.transform.position);
                float distanceRatio = Mathf.Clamp01(distance / damageRadius); // 0 en centro, 1 en borde

                // Interpolar daño linealmente (más daño cerca)
                float damageScale = Mathf.Lerp(1f, minDamagePercent, distanceRatio);
                float finalDamage = maxDamage * damageScale;

                playerHealth.TakeDamage(finalDamage);
                */
                //Debug.Log($"Player hit by AoE: DistanceRatio={distanceRatio}, Damage={finalDamage}");
                Debug.Log($"Player hit by AoE");
                 // TODO: Play Hit Player SFX/VFX (Quizás uno diferente para AoE)
            }
        }
    }
