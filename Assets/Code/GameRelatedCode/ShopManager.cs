using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private ShopManager _instance;

    public ShopManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ShopManager>(); //Intenta buscar uno
            }
            return _instance; //Ahora tengo un singleton.
        }
    }

    public void Start()
    {
        
    }

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
