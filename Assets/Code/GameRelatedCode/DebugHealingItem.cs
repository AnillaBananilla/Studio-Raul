using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHealingItem : Item
{
    public string name = "DebugApple";
    public int price = 10;
    public int quantity = 0;
    public int maxStack = 99;

    public override void UseItem()
    {
        if (quantity >= 0)
        {
            GameManager.instance.healtAmount += 20;
            quantity -= 1;
        }
    }

    public void Restock(int amount)
    {
        quantity += amount;
        if (quantity > maxStack)
        {
            quantity = maxStack;
        }
    }
}
