using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform attackCheck;
    public float attackRadius = 2.0f;
    public LayerMask enemyLayer;
    public Animator animator;
    public GameManager gameManager;
    public GameObject bulletPrefab;
    public Transform Spawnpoint;
    //public MetroidCharacterController2D controller;



    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive == false)
        {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("Attack_Trigger");
            Collider2D[] enemies =  Physics2D.OverlapCircleAll(attackCheck.position, attackRadius, enemyLayer);
            for (int counter = 0; counter < enemies.Length; counter++)
            {
                enemies[counter].GetComponent<SpriteRenderer>().color = Color.red;
                enemies[counter].GetComponent<Healt>().Damage(1);
            }
        }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
            animator.SetTrigger("Attack_Trigger");
            // GameObject newBullet = GameObject.Instantiate(bulletPrefab);
            //newBullet.transform.position = Spawnpoint.position;

            Droplets newBullet = null;
            PoolManager.Instance.SpawnObject<Droplets>(out newBullet, bulletPrefab, Spawnpoint.position, Spawnpoint.rotation, PoolManager.PoolType.GameObjects);
            // Determinar la direcci�n de disparo
            ///Vector2 shootDirection = controller.IsFacingRight() ? Vector2.right : Vector2.left;

            // Agregar una fuerza en Y, por ejemplo, hacia arriba
            float verticalForce = 0.2f; // Ajusta este valor seg�n necesites
            ///shootDirection += Vector2.up * verticalForce;

            // Normalizar la direcci�n si es necesario
            ///shootDirection.Normalize();

                    // Aplicar la fuerza al proyectil
                    newBullet.GetComponent<Rigidbody2D>().AddForce(shootDirection * 2000.0f);
                    gameManager.usePinture(20);
                }
                else
                {
                    animator.SetTrigger("Attack_Trigger");
                    Collider2D[] enemies = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius, enemyLayer);
                    for (int counter = 0; counter < enemies.Length; counter++)
                    {
                        enemies[counter].GetComponent<SpriteRenderer>().color = Color.red;
                        enemies[counter].GetComponent<Healt>().Damage(1);
                    }
                }
            }
            else
            {
                return;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }

}
