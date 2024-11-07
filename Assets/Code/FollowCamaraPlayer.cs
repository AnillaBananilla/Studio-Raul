using UnityEngine;

public class FondoSeguirPlayer : MonoBehaviour
{
    public Transform player; // Asigna el Player en el Inspector
    public float offsetY = 0f; // Ajuste en Y para posicionar el fondo m�s arriba o abajo

    void Update()
    {
        // Iguala la posici�n X e Y del fondo a la del Player, con un ajuste en Y
        transform.position = new Vector3(player.position.x, player.position.y + offsetY, transform.position.z);
    }
}