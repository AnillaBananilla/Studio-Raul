using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int Damage;
    public float Recoilforce;
    private Vector3 direction;
    public LayerMask Playerlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject Player = collision.gameObject;
            Player.GetComponent<Healt>().Damage(Damage);

            // Cambia el color a rojo
            SpriteRenderer playerSprite = Player.GetComponent<SpriteRenderer>();
            playerSprite.color = Color.red;

            // Aplica la fuerza de retroceso
            direction = (Player.transform.position - transform.position).normalized;
            Player.GetComponent<Rigidbody2D>().velocity = direction * Recoilforce;

            // Inicia la corrutina para volver el color a blanco después de 1 segundo
            StartCoroutine(ResetColor(playerSprite));
        }
    }

    // Corrutina que espera 1 segundo y luego restablece el color a blanco
    IEnumerator ResetColor(SpriteRenderer playerSprite)
    {
        yield return new WaitForSeconds(1f);
        playerSprite.color = Color.white;
    }
}
