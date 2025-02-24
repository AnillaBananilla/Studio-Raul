using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyItemEvent : GameEvent
{
    public Item Item;
    public BuyItemEvent(params object[] _list) : base(_list)
    {

    }
}
