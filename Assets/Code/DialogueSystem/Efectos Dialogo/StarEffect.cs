using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class StarEffect : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private float timeOffset;
    public float frequency = 1f;      // Velocidad de parpadeo
    public float amplitude = 0.5f;    // Cuánto varía la opacidad
    public float baseAlpha = 0.5f;    // Opacidad mínima

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        timeOffset = Random.Range(0f, 10f);  // 🎯 Desfase único por estrella
    }

    void Update()
    {
        float alpha = baseAlpha + Mathf.Sin((Time.unscaledTime + timeOffset) * frequency) * amplitude;
        canvasGroup.alpha = Mathf.Clamp01(alpha);
    }
}
