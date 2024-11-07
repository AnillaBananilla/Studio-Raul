using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tienda : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject shopScreen;
    // Start is called before the first frame update
     private bool isPlayerInside = false;


    void Update()
    {
        // Verifica si el jugador está dentro del collider y presiona la tecla 'B'
        if (isPlayerInside && Input.GetKeyDown(KeyCode.B))
        {
            titleScreen.SetActive(false);
            shopScreen.SetActive(true);
            Debug.Log("no");
        }
        if (isPlayerInside && Input.GetKeyDown(KeyCode.Escape))
        {
            shopScreen.SetActive(false);
            titleScreen.SetActive(true);
            Debug.Log("Adios");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que entra al collider es el jugador
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
 
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // Verifica si el objeto que entra al collider es el jugador
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;

        }
    }
}
