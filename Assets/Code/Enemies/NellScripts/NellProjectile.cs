using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NellProjectile : MonoBehaviour
{
    private Transform player;
    public float speed;
    Rigidbody2D projectileRb;

    //this method activates when the bullets spawns and it will calculate the trajectory once so
    //that it goes to the player, but it is only once so that the player can dodge the bullet
    //after some time, the bullet gets destroyed
    void Start()
    {
        projectileRb = GetComponent<Rigidbody2D>();
        if(player != null){
            Vector2 moveDir = (player.position - transform.position).normalized * speed;
            projectileRb.velocity = moveDir;
        }
        Destroy(this.gameObject, 2);
    }

    internal void SetTarget(Transform playerTransform)
    {
        player = playerTransform;
    }
}
