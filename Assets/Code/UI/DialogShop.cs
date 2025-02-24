using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogShop : MonoBehaviour
{
    public GameObject titleScreen;
    public Dialog typewriterEffect; // Referencia al script del efecto
    public TextMeshProUGUI dialogueText; // Referencia al texto donde se mostrar� el di�logo
    public string[] messages; // Array de mensajes
    private int currentDialogueIndex = 0; // �ndice del mensaje actual
    private bool isPlayerInside = false; // Verifica si el jugador est� en el �rea

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            titleScreen.SetActive(true);
            ShowNextDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            titleScreen.SetActive(false);
            currentDialogueIndex = 0; // Reiniciar di�logo al salir
        }
    }
    void Start()
    {
        messages = new string[]
        {
        "�Bienvenido a la tienda, viajero!",
        "Aqu� encontrar�s pociones y espadas.",
        "Vuelve pronto y que tengas una gran aventura."
        };
    }

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.Return)) // Detectar Enter
        {
            ShowNextDialogue();
        }
    }

    private void ShowNextDialogue()
    {
        if (messages.Length == 0) return;

        if (typewriterEffect != null)
        {
            typewriterEffect.StartTypewriter(messages[currentDialogueIndex]);
        }

        currentDialogueIndex++;

        if (currentDialogueIndex >= messages.Length)
        {
            currentDialogueIndex = 0; // Reinicia el di�logo si llega al final
        }
    }
}