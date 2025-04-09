using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushAttackHitbox : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerEntity player = collision.gameObject.GetComponent<PlayerEntity>();
            player.TakeDamage(15);
        }
    }
}

