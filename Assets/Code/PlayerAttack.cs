using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem.XR;
using Unity.VisualScripting;

public class PlayerAttack : MonoBehaviour
{
    public InputHandler inputHandler;

    public Healt PlayerHP;

    public Transform attackCheck;
    public float attackRadius = 2.0f;
    public LayerMask enemyLayer;
    public Animator animator;
    public GameManager gameManager;
    public GameObject bulletPrefab;
    public Transform Spawnpoint;
    public Vector2 shootDirection;
    private enum ColorState { Azul, Amarillo, Rojo } // Enumerador de colores
    private ColorState currentColor = ColorState.Azul; // Color inicial
    public Image imageToChange; // Arrástralo desde el Inspector
    int valueToPrint = 0;
    public Animator[] animators; // Array de Animators de los sprites
    private int currentIndex = 0; // Índice del sprite actual


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
            if (inputHandler.attack)
            {
                animator.SetTrigger("Attack_Trigger");
                Collider2D[] enemies = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius, enemyLayer);

                for (int counter = 0; counter < enemies.Length; counter++)
                {
                    enemies[counter].GetComponent<SpriteRenderer>().color = Color.red;
                    enemies[counter].GetComponent<Healt>().Damage(25);

                    // Obtener la posición del enemigo
                    Vector3 enemyPosition = enemies[counter].transform.position;

                    // Instanciar un objeto en esa posición
  
                }
            }

            if (inputHandler.attackPaint)
            {   //si sigue habiendo pintura, entonces se realizan estas acciones
                if (gameManager.pintureAmount > 0)
                {   //se inicia la animación
                    animator.SetTrigger("Attack_Trigger");
                    gameManager.usePinture(1);
                    Droplets newBullet = null;
                    //el pool manager spawnea alguna bala
                    PoolManager.Instance.SpawnObject<Droplets>(out newBullet, bulletPrefab, Spawnpoint.position, Spawnpoint.rotation, PoolManager.PoolType.GameObjects);

                    //en caso de existir la bala y no ser null se hace lo siguiente
                    if (newBullet != null)
                    {
                        //se obtiene el RB de la bala
                        Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
                        //al detectarlo, entonces se realizan los pasos de detectar la dirección 
                        // del personaje y aplicarle fuerza en esa dirección a la bala
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
                else
                {
                    animator.SetTrigger("Attack_Trigger");
                    Collider2D[] enemies = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius, enemyLayer);

                    for (int counter = 0; counter < enemies.Length; counter++)
                    {
                        enemies[counter].GetComponent<SpriteRenderer>().color = Color.red;
                        enemies[counter].GetComponent<Healt>().Damage(25);

                        // Obtener la posición del enemigo
                        Vector3 enemyPosition = enemies[counter].transform.position;

                    }
                }
            }
        
        if (inputHandler.changeColor)
        {
            ChangeColor();
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
    private void ChangeColor()
    {
       
        currentColor = (ColorState)(((int)currentColor + 1) % 3);
        PlayNextAnimation();
        switch (currentColor)
        {
            case ColorState.Azul:
                valueToPrint = 10;
                Debug.Log("Valor impreso: " + valueToPrint);
                imageToChange.color = Color.cyan;
                break;
            case ColorState.Amarillo:
                valueToPrint = 20;
                Debug.Log("Valor impreso: " + valueToPrint);
                imageToChange.color = Color.magenta;
                break;
            case ColorState.Rojo:
                valueToPrint = 30;
                Debug.Log("Valor impreso: " + valueToPrint);
                imageToChange.color = Color.yellow;
                break;
        }
    }
    public string GetCurrentColor()
    {
        return currentColor.ToString();
    }
    void PlayNextAnimation()
    {
        // Obtener la gota anterior antes de cambiar el índice
        int previousIndex = currentIndex;

        // Avanzar al siguiente sprite en la secuencia
        currentIndex = (currentIndex + 1) % animators.Length;

        // Asegurar que los triggers anteriores no interfieran
        animators[currentIndex].ResetTrigger("decrecerGota");
        animators[previousIndex].ResetTrigger("crecerGota");

        // Activar la animación de crecimiento de la nueva gota
        animators[currentIndex].SetTrigger("crecerGota");

        // Activar la animación de reducción de la gota anterior
        animators[previousIndex].SetTrigger("decrecerGota");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("NellProjectile")){
            //gameManager.takeDamage(25);
            PlayerHP.Damage(25);
            GameManager.instance.lifeBar.fillAmount = PlayerHP.currentHealt / 100F;
        }
        /*else if(collision.gameObject.CompareTag("ArbolilloPunch")){
            gameManager.takeDamage(15);
        }*/
    }

}
