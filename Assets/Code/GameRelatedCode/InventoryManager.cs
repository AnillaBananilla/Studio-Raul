using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{


    public List<Item> MyItems; //?
    public Item EquippedItem;

    private static InventoryManager _instance;

    public static InventoryManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<InventoryManager>(); //Intenta buscar uno
            }
            return _instance; //Ahora tengo un singleton.
        }
    }

    void Start()
    {
        //EventManager.m_Instance.AddListener<BuyItemEvent>(AddItem);
        EventManager.m_Instance.AddListener<EquipItemEvent>(EquipItem);


        MyItems = new List<Item>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(Item _item)
    {
        MyItems.Add(_item);
    }

    public void EquipItem(EquipItemEvent _e)
    {
        EquippedItem = _e.eventItem;
    }
}
