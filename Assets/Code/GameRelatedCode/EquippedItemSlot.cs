using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedItemSlot : MonoBehaviour
{
    public Item _Item;

    public void SetItem(Item _item )
    {
        _Item = _item;
    }
}
