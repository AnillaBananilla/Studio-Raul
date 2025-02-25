using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGBPlatform : MonoBehaviour
{
    public char Color;

    private Transform child;

    public void Start()
    {
       child =  this.gameObject.transform.Find("Square");
    }

    public void Update()
    {
        if (World1_Manager.Instance.CurrentColor() == Color)
        {
            child.gameObject.SetActive(true);
        }
        else
        {
            child.gameObject.SetActive(false);
        }
    }
}
