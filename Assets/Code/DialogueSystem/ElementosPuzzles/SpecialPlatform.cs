using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPlatform : MonoBehaviour
{
    [Header("Assets de plataformas para asignar")]
    public GameObject walkablePlatform;
    public GameObject transparentPlatform;

    [Header("Delays para activación/desactivación")]
    public float  activeDuration = 5f;
    public float reactivateDelay = 3f;

    [Header("Efecto de temblor")]
    public float shakeDuration = 2f;
    public float shakeAmount = 0.05f;

    private bool isActivated = false;
    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = walkablePlatform.transform.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !isActivated){
            StartCoroutine(HandlePlatformTimer());
        }   
    }

    private IEnumerator HandlePlatformTimer(){
        isActivated = true;

        walkablePlatform.SetActive(true);
        transparentPlatform.SetActive(false);

        yield return new WaitForSeconds(activeDuration - shakeDuration);

        StartCoroutine(ShakePlatform());

        yield return new WaitForSeconds(shakeDuration);

        walkablePlatform.SetActive(false);
        transparentPlatform.SetActive(true);

        yield return new WaitForSeconds(reactivateDelay);

        walkablePlatform.SetActive(true);
        transparentPlatform.SetActive(false);

        walkablePlatform.transform.localPosition = originalPosition;

        isActivated = false;
    }

    private IEnumerator ShakePlatform()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitCircle * shakeAmount;
            walkablePlatform.transform.localPosition = originalPosition + randomOffset;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Restaurar posición exacta
        walkablePlatform.transform.localPosition = originalPosition;
    }
}
