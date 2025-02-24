using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item
{
    public string name;
    public int price;
    public int quantity;
    public int maxStack;

    public Image itemImage;

    public abstract void UseItem(); //To be overriden

  
}
