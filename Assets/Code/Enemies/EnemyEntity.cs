using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : Entity
{
    public override void Attack()
    {
        
    }

    public override void Die()
    {
        
    }

    public override void TakeDamage(int Damage, char color)
    {
        if (!IsImmune)
        {
            HP -= Damage;
            IsImmune = true;
            
            if (HP <= 0)
            {
                Die();
            }

            StartCoroutine(Immunity(2));
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
