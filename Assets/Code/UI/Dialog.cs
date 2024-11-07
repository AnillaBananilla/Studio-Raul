using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    public GameObject titleScreen;


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que entra al collider es el jugador
        if (other.CompareTag("Player"))
        {
            titleScreen.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // Verifica si el objeto que entra al collider es el jugador
        if (other.CompareTag("Player"))
        {
            titleScreen.SetActive(false);
        }
    }

}
