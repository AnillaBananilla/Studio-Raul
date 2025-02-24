using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScreen : MonoBehaviour
{
    private Item HealingBottle = new DebugHealingItem();
    public void SelectHealingItem()
    {
        if (GameManager.instance.score >= HealingBottle.price)
        {
            GameManager.instance.score -= HealingBottle.price;
            InventoryManager.instance.AddItem(HealingBottle);
        }
    }

    public void ExitShop()
    {
        GameManager.instance.CloseMenu();
    }
}
