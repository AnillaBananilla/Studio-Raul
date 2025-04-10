using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackReceiver : MonoBehaviour
{
    [SerializeField] private Entity _entity;
   public void ReceiveDamage(int Damage, char color)
    {
        _entity.TakeDamage(Damage, color);
    }

    public void ReceiveDamage(int Damage)
    {
        _entity.TakeDamage(Damage);
    }
}
