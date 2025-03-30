using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NellAttack : MonoBehaviour
{
    public GameObject projectile;
    public GameObject projectileOrigin;
    public float fireRate = 1f;
    private float nextFireTime;
    public Transform player;
    

    public void Shoot(){
        if(nextFireTime < Time.time){
            GameObject bullet = Instantiate(projectile,projectileOrigin.transform.position, Quaternion.identity);
            NellProjectile script = bullet.GetComponent<NellProjectile>();
            script.SetTarget(player);
            nextFireTime = Time.time + fireRate;
        }
    }
}
