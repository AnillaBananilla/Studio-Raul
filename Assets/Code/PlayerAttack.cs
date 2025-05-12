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
    [Header("Manager del input del jugador")]
    public InputHandler inputHandler;

    public Healt PlayerHP;

    public Transform attackCheck;
    public float attackRadius = 2.0f;

    [Header("Capas que afecta el ataque")]
    public LayerMask enemyLayer;
    public LayerMask buttonLayer;
    public LayerMask buttonsRoom7;
    public Animator animator;
    public GameManager gameManager;
    [Header("Info de ataques con balas de pintura")]
    public GameObject bulletPrefab;
    public Transform Spawnpoint;
    public Vector2 shootDirection;
    private enum ColorState { Azul, Amarillo, Magenta } // Enumerador de colores
    private ColorState currentColor = ColorState.Azul; // Color inicial
    public Image imageToChange; // Arrástralo desde el Inspector
    public Image rainDropToChange; // Arrástralo desde el Inspector
    public Image rainDropToChange1; // Arrástralo desde el Inspector
    public Image rainDropToChange2; // Arrástralo desde el Inspector
    int valueToPrint = 0;
    public Animator[] animators; // Array de Animators de los sprites
    private int currentIndex = 0; // Índice del sprite actual
    public Sprite blue;
    public Sprite yellow;
    public Sprite magenta;
    public Sprite rainDropblue;
    public Sprite rainDropyellow;
    public Sprite rainDropmagenta;
    private float lastAttackTime = 0f;
    public float doubleAttackThreshold = 0.3f;
    private float doubleTapThreshold = 0.3f;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //     if (inputHandler.attack)
        //   {
        // animator.SetTrigger("Attack_Trigger");
        //daño a enemigos
        // Collider2D[] enemies = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius, enemyLayer);

        // for (int counter = 0; counter < enemies.Length; counter++)
        // {

        //    enemies[counter].GetComponent<SpriteRenderer>().color = Color.red;
        //    enemies[counter].GetComponent<Healt>().Damage(25);
        // }
        //activación de botones/interacción
        // Collider2D[] buttons = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius, buttonLayer);
        // for(int i = 0; i < buttons.Length; i++){
        //     ButtonDoors button = buttons[i].GetComponent<ButtonDoors>();
        //     if(button != null){
        //        Debug.Log("Botón detectado por ataque.");
        //       button.Activate();
        //   }
        // }
        // }

        if (inputHandler.attack)
        {
            float currentTime = Time.time;

            if (currentTime - lastAttackTime < doubleTapThreshold)
            {
                // Ataque largo (doble toque)
                animator.SetTrigger("Attack");
            }
            else
            {
                // Ataque rápido (un solo toque)
                animator.SetTrigger("AttackLong");
            }

            lastAttackTime = currentTime;

            // Daño a enemigos (esto puede mantenerse igual)
            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius, enemyLayer);
            for (int counter = 0; counter < enemies.Length; counter++)
            {
                enemies[counter].GetComponent<SpriteRenderer>().color = Color.red;
                enemies[counter].GetComponent<Healt>().Damage(25);
            }

            // Activación de botones/interacción
            Collider2D[] buttons = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius, buttonLayer);
            for (int i = 0; i < buttons.Length; i++)
            {
                ButtonDoors button = buttons[i].GetComponent<ButtonDoors>();
                if (button != null)
                {
                    Debug.Log("Botón detectado por ataque.");
                    button.Activate();
                }
            }

            Collider2D[] room7buttons = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius, buttonsRoom7);
            for (int i = 0; i < room7buttons.Length; i++)
            {
                PuzzleButtons button = room7buttons[i].GetComponent<PuzzleButtons>();
                if (button != null)
                {
                    Debug.Log("Botón cuarto 7 activado");
                    button.toggleColor();
                }
            }
        }


        if (inputHandler.attackPaint)
        {   //si sigue habiendo pintura, entonces se realizan estas acciones
            float CurrentPaint = gameManager.paintAmount[gameManager.paintColorIndex];
            if (CurrentPaint > 0)
            {   //se inicia la animación
                animator.SetTrigger("Attack_Trigger");
                gameManager.usePinture(5);
                Droplets newBullet = null;
                //el pool manager spawnea alguna bala
                PoolManager.Instance.SpawnObject<Droplets>(out newBullet, bulletPrefab, Spawnpoint.position, Spawnpoint.rotation, PoolManager.PoolType.GameObjects); //Checar aca

                //en caso de existir la bala y no ser null se hace lo siguiente
                if (newBullet != null)
                {
                    newBullet.UpdateColor();
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
                imageToChange.sprite = blue;
                rainDropToChange.sprite = rainDropblue;
                rainDropToChange1.sprite = rainDropmagenta;
                rainDropToChange2.sprite = rainDropyellow;
                GameManager.instance.paintColorIndex = 0;
                break;
            case ColorState.Amarillo:
                valueToPrint = 20;
                imageToChange.sprite = yellow;
                rainDropToChange.sprite = rainDropmagenta;
                rainDropToChange1.sprite = rainDropyellow;
                rainDropToChange2.sprite = rainDropblue;
                GameManager.instance.paintColorIndex = 1;
                break;
            case ColorState.Magenta:
                valueToPrint = 30;
                imageToChange.sprite = magenta;
                rainDropToChange.sprite = rainDropyellow;
                rainDropToChange1.sprite = rainDropblue;
                rainDropToChange2.sprite = rainDropmagenta;
                GameManager.instance.paintColorIndex = 2;
                break;
        }
        gameManager.usePinture(0);
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
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NellProjectile"))
        {
            //gameManager.takeDamage(25);
            PlayerHP.Damage(25);
            //GameManager.instance.takeDamage(25);
            //GameManager.instance.lifeBar.fillAmount = PlayerHP.currentHealt / (float)PlayerHP.maxHealt;
        }
    }
}