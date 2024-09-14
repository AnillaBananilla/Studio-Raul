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

    private bool[] attackCollection = {true, false}; // 0 = melee, 1 = ranged
    private int selectedAttack;

    public string CurrentAttack
    {
        get
        {
            switch (selectedAttack)
            {
                case 0:
                    return "Melee";
                case 1:
                    return "Ranged";
                default:
                    return "Melee";
            }
        }
    }

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
            MeleeAttack();
        }
        }
        else
        {
            return; // termina la funcion
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {

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
        BulletClone.GetComponent<Rigidbody2D>().AddForce(spawnpoint.forward * 1000.0f);
    }
}
