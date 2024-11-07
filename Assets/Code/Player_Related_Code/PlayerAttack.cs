using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform attackCheck;
    public float attackRadius = 2.0f;
    public LayerMask enemyLayer;
    public Animator animator;
    public GameManager gameManager;

    [SerializeField] private GameObject TheBullet;
    [SerializeField] Transform spawnpoint;
    private int selectedAttack;


    void Start()
    {
        selectedAttack = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive == true)
        {

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                switch (selectedAttack)
                {
                    case 0:
                        MeleeAttack();
                        break;

                    case 1:
                        Shoot();
                        break;

                }
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                changeAttack();
            }
        }

        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }

    private void MeleeAttack()
    {
        animator.SetTrigger("Attack_Trigger");
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius, enemyLayer);
        for (int counter = 0; counter < enemies.Length; counter++)
        {
            enemies[counter].GetComponent<SpriteRenderer>().color = Color.red;
            enemies[counter].GetComponent<Healt>().Damage(1);
        }
    }

    private void Shoot()
    {
        animator.SetTrigger("Attack_Trigger");
        GameObject BulletClone = GameObject.Instantiate(TheBullet);
        BulletClone.transform.position = spawnpoint.position;
        BulletClone.transform.rotation = spawnpoint.rotation;
    }

    private void changeAttack()
    {
        switch (selectedAttack)
        {
            case 0:
                if (GameManager.instance.RangeAttack)
                {
                    selectedAttack = 1;
                    Debug.Log("SHOOT");
                }

                break;

            case 1:
                selectedAttack = 0;
                Debug.Log("PUNCH");
                break;
        }
    }
}
