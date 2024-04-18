using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform attackCheck;
    public float attackRadius = 2.0f;
    public LayerMask enemyLayer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
          Collider2D[] enemies =  Physics2D.OverlapCircleAll(attackCheck.position, attackRadius, enemyLayer);
            for (int counter = 0; counter < enemies.Length; counter++)
            {
                enemies[counter].GetComponent<SpriteRenderer>().color = Color.red;
                enemies[counter].GetComponent<Healt>().Damage(1);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }
}
