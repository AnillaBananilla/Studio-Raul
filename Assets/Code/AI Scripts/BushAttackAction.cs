using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushAttackAction : AIAction
{
    public float speed;

    public override void PerformAction()
    {

        if (_brain.Target != null)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, _brain.Target.position, step);
            //transform.LookAt(_brain.Target.position);
        } else
        {
            Debug.Log("TARGET NOT FOUND");
        }
            Debug.Log("ATTACKING");
    }
}
