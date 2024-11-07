using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAttack : MonoBehaviour
{

    public GameObject puddlePrefab; // Prefab del charco
    public float puddleDuration = 5f; // Tiempo que dura el charco
    public float puddleOffsetY = -5f; // Valor para bajar el charco en Y
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificamos si el objeto con el que colisionamos tiene el tag "Player"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Intentamos obtener el SpriteRenderer del objeto con el que colisionamos
            SpriteRenderer otherSpriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
            Healt otherSpriteRendererHealt = collision.gameObject.GetComponent<Healt>();

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
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            Vector3 puddlePosition = new Vector3(transform.position.x, transform.position.y + puddleOffsetY, transform.position.z);
            GameObject puddle = Instantiate(puddlePrefab, puddlePosition, Quaternion.identity);
            Destroy(puddle, puddleDuration); // Destruir el charco después de X tiempo
        }
        Destroy(gameObject);
    }
}