using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogShop : MonoBehaviour
{
    public GameObject titleScreen; //Es la referencia al Transform vacio de la escena,se usa paraactivarlo y desactivarlo
    public TypewriterEffect typewriterEffect; // Referencia al script del efecto
    public TextMeshProUGUI dialogueText; // Referencia al texto donde se mostrar� el di�logo
    private string[] messages; // Array de mensajes
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
        "�Hola viajero, como te ecuentras!?",
        "Te ves algo decaido, pero no te preocupes",
        "yo te dare una mision para que tengas una gran aventura!"
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

        if (typewriterEffect != null)
        {
            typewriterEffect.StartTypewriter(messages[currentDialogueIndex]);
        }

        currentDialogueIndex++;

        if (currentDialogueIndex >= messages.Length)
        {
            titleScreen.SetActive(false);
            isPlayerInside = false; // Para que no se pueda seguir presionando Enter
        }
    }
}