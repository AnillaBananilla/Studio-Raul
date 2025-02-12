using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAttack : MonoBehaviour
{
    public GameObject puddlePrefab; // Prefab del charco
    public float puddleDuration = 5f; // Tiempo que dura el charco
    public float puddleOffsetY = -5f; // Valor para bajar el charco en Y
    private SpriteRenderer spriteRenderer;
    private Player playerReference; // Referencia al script Player

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            playerReference = playerObject.GetComponent<Player>();
        }
        else
        {
            Debug.LogError("No se encontró el objeto Player en la escena.");
        }

        UpdateColor();
    }

    private void UpdateColor()
    {
        if (playerReference == null || spriteRenderer == null) return;

        switch (playerReference.GetCurrentColor())
        {
            case "Azul":
                spriteRenderer.color = Color.blue;
                break;
            case "Amarillo":
                spriteRenderer.color = Color.yellow;
                break;
            case "Rojo":
                spriteRenderer.color = Color.red;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SpriteRenderer otherSpriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
            Healt otherSpriteRendererHealt = collision.gameObject.GetComponent<Healt>();

            if (otherSpriteRenderer != null)
            {
                otherSpriteRenderer.color = Color.cyan;
                otherSpriteRendererHealt.Damage(1);
            }

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            Vector3 puddlePosition = new Vector3(transform.position.x, transform.position.y + puddleOffsetY, transform.position.z);
            GameObject puddle = Instantiate(puddlePrefab, puddlePosition, Quaternion.identity);

            // Obtener el SpriteRenderer del puddle y asignarle el color del BallAttack
            SpriteRenderer puddleSpriteRenderer = puddle.GetComponent<SpriteRenderer>();
            if (puddleSpriteRenderer != null)
            {
                puddleSpriteRenderer.color = spriteRenderer.color;
            }

            Destroy(puddle, puddleDuration);
        }

        Destroy(gameObject);
    }
}
