using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushAttackDecision : AIDecision
{
    public bool wasAttacked = false;
    public int TouchCounter = 0;
    public override bool Decide()
    {
        return wasAttacked;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerProjectile")
        {
            Debug.Log("OUCH!");
            TouchCounter++;
            Debug.Log(TouchCounter);
            if (TouchCounter >= 3)
            {
                _brain.Target = collision.transform;
                wasAttacked = true;
            }
            
        }
    }

    public void TookDamage()
    {
        TouchCounter++;
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        wasAttacked = false;
        TouchCounter = 0;
    }

}
