using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arbolillo : Entity
{
    public Collider2D AttackHitbox;
    [SerializeField] private BushAttackDecision AttackDecision;
    public override void Attack()
    {
        AttackHitbox.gameObject.SetActive(true);
        StartCoroutine(DeactivateHitbox());
    }

    public override void Die()
    {
        
    }

    public override void TakeDamage(int Damage, char color)
    {
        base.TakeDamage(Damage, color);
        AttackDecision.TookDamage();
        Debug.Log("I took damage");
    }

    // Start is called before the first frame update
    void Start()
    {
        AttackHitbox.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator DeactivateHitbox()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            AttackHitbox.gameObject.SetActive(false);

        }
    }
}
