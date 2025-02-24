using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public object[] Items = new object[1];
   
    public void SaveItem(Item _item)
    {
        Items[0] = _item;
    }

    public void EquipItem()
    {
        EventManager.m_Instance.InvokeEvent<EquipItemEvent>(new EquipItemEvent(Items));
    }
}
