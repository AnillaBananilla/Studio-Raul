using System.Collections;
using UnityEngine;
using TMPro; // Si usas TextMeshPro

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // Referencia al TextMeshPro UI
    public float typeSpeed = 0.05f; // Velocidad de escritura

    public void StartTypewriter(string message)
    {
        StopAllCoroutines(); // Detener cualquier otro diálogo en curso
        StartCoroutine(TypeText(message));
    }

    private IEnumerator TypeText(string message)
    {
        dialogueText.text = ""; // Limpiar el texto antes de comenzar
        foreach (char letter in message)
        {
            dialogueText.text += letter; // Agregar letra por letra
            yield return new WaitForSecondsRealtime(typeSpeed); // Pausa entre letras
            /* NOTA
             * Cambié WaitForSeconds a WaitForSecondsRealtime para que el efecto siga ocurriendo
             * Aunque TimeScale sea 0
             *  - Emi
             */
        }
    }

}