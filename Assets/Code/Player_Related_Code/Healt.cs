using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healt : MonoBehaviour
{
    // Start is called before the first frame update
    private int currentHealt;
    public int maxHealt;
    void Start()
    {
        currentHealt = maxHealt;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Damage(int damage)
    {
        currentHealt -= damage;
        if (currentHealt <= 0)
        {
            Die();
        }
    }
    public void Heal(int heal)
    {
        currentHealt += heal;
        if (currentHealt > maxHealt)
        {
            currentHealt = maxHealt;
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }

}
