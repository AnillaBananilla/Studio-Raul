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
        EventManager.m_Instance.AddListener<CanvasColorChangeEvent>(Activate);
        if (World1_Manager.Instance.CurrentColor() == Color && child != null)
        {
            child.gameObject.SetActive(true);
        }
        else
        {
            child.gameObject.SetActive(false);
        }
    }

    public void Update()
    {

    }

    public void Activate(CanvasColorChangeEvent e)
    {
        if(child != null)
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
}
