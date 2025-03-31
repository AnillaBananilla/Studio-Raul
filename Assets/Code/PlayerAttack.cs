using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public InputHandler inputHandler;

    // Start is called before the first frame update
    public Transform attackCheck;
    public float attackRadius = 2.0f;
    public LayerMask enemyLayer;
    public Animator animator;
    public GameManager gameManager;
    public GameObject bulletPrefab;
    public Transform Spawnpoint;
    public Vector2 shootDirection;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive)
        {

            if (inputHandler.attack)
            {
                animator.SetTrigger("Attack_Trigger");


                Collider2D[] enemies = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius, enemyLayer);
                for (int counter = 0; counter < enemies.Length; counter++)
                {
                    enemies[counter].GetComponent<EnemyEntity>().TakeDamage(5, 'r');
                    //enemies[counter].GetComponent<SpriteRenderer>().color = Color.red;
                    

                }
            }


            //esto no debe estar, está obsoleto, debe ser como 
            // el "inputHandler.attack" del ataque de arriba
            if (inputHandler.attackPaint)
            {
                animator.SetTrigger("Attack_Trigger");

                Droplets newBullet = null;
                PoolManager.Instance.SpawnObject<Droplets>(out newBullet, bulletPrefab, Spawnpoint.position, Spawnpoint.rotation, PoolManager.PoolType.GameObjects);

                if (newBullet != null)
                {
                    Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        // Detectar dirección del personaje
                        float direction = transform.localScale.x > 0 ? 1f : -1f;

                        // Aplicar fuerza en la dirección correcta
                        Vector2 forceDirection = new Vector2(1f * direction, 0.5f) * 20f;
                        rb.AddForce(forceDirection, ForceMode2D.Impulse);
                    }
                }
            }
        }
        else
        {
            return;
        }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
        }

}
