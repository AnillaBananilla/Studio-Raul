using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int HP;
    public int ATT;
    public bool IsImmune = false;

    public SpriteRenderer image;

    public abstract void Attack();
    public virtual void TakeDamage(int Damage, char color)
    {

        if (!IsImmune)
        {
            HP -= Damage;
            IsImmune = true;

            if (HP <= 0)
            {
                Die();
            }
            this.GetComponent<SpriteRenderer>().color = Color.red;

            StartCoroutine(Immunity(2));
        }
    }

    public virtual void TakeDamage(int Damage)
    {
        if (!IsImmune)
        {
            HP -= Damage;
            IsImmune = true;

            if (HP <= 0)
            {
                Die();
            }
            this.GetComponent<SpriteRenderer>().color = Color.red;

            StartCoroutine(Immunity(1));
        }
    }
    

    public abstract void Die();

    protected IEnumerator Immunity(int iSeconds) //Sets IsImmune back to false.
    {
        while (true)
        {
            yield return new WaitForSeconds(iSeconds);
            IsImmune = false;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frameS
    void Update()
    {
        
    }
}
