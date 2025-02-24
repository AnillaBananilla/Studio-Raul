using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItemEvent : GameEvent
{
    public Item eventItem;
    public EquipItemEvent(params object[] _list) : base(_list)
    {

        eventItem = (Item)_list[0]; //aqui guardamos el item. (hay que castearlo)
    }
}
