using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int HP;
    public int ATT;
    public bool IsImmune = false;

    public abstract void Attack();
    public abstract void TakeDamage(int Damage, char color);

    public abstract void Die();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frameS
    void Update()
    {
        
    }
}
