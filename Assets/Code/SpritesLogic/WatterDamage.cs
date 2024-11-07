using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatterDamage : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que entra al collider es el jugador
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Jugador entró al área.");
            // Intentamos obtener el SpriteRenderer del objeto con el que colisionamos
            // Intentamos obtener el SpriteRenderer del objeto con el que colisionamos
            SpriteRenderer otherSpriteRenderer = other.gameObject.GetComponent<SpriteRenderer>();
            Healt otherSpriteRendererHealt = other.gameObject.GetComponent<Healt>();

            // Si el objeto con el que colisionamos tiene un SpriteRenderer, cambiamos su color a rojo
            if (otherSpriteRenderer != null)
            {
                otherSpriteRenderer.color = Color.cyan; // Cambia el color del objeto colisionado (Player)
                otherSpriteRendererHealt.Damage(1);
            }
            else
            {
                return;
            }

        }
    }
}
