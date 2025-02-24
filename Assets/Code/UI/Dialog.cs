using System.Collections;
using UnityEngine;
using TMPro; // Si usas TextMeshPro

public class Dialog : MonoBehaviour
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
            yield return new WaitForSeconds(typeSpeed); // Pausa entre letras
        }
    }
}