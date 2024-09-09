using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int Damage;
    public float Recoilforce;
    private Vector3 direction;
    public LayerMask Playerlayer;
    public Animator animator;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetTrigger("Damage_Trigger");
            GameObject Player = collision.gameObject;
            Player.GetComponent<Healt>().Damage(Damage);
            direction = (Player.transform.position - transform.position).normalized;
            Player.GetComponent<Rigidbody2D>().velocity = direction * Recoilforce;




        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
