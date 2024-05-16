using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeInDuration = 1f;
    private float currentAlpha = 0f;
    private float fadeSpeed;

    void Start()
    {
        fadeSpeed = 1f / fadeInDuration;
        canvasGroup.alpha = 0f;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        while (currentAlpha < 1f)
        {
            currentAlpha += fadeSpeed * Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(currentAlpha);
            yield return null;
        }
    }
}
