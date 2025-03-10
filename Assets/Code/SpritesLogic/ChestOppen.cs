using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOppen : MonoBehaviour
{
    private GameManager gameManagerX;
    public GameObject prefab;
    private bool isPlayerNear = false; // Variable para saber si el jugador está cerca del cofre
    public Animator anim;
    public float prefabOffsetY; // Valor para bajar el charco en Y
    public float prefabOffsetX; // Valor para bajar el charco en Y

    void Start()
    {
        gameManagerX = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        // Verifica si el jugador está cerca y presiona la tecla "O"
        if (isPlayerNear && Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetTrigger("chestTrigger");
            Vector3 puddlePosition = new Vector3(transform.position.x + prefabOffsetX, transform.position.y + prefabOffsetY, transform.position.z);
            GameObject puddle = Instantiate(prefab, puddlePosition, Quaternion.identity);

           
            Debug.Log("Cofre abierto");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;  // El jugador está cerca, podemos recibir el input
            Debug.Log("Jugador cerca del cofre");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = false; // El jugador se aleja del cofre
        }
    }
}