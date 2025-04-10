using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTile : MonoBehaviour
{
    public int ID;

    private Image image;
    public bool crossed;
    void Start()
    {
        crossed = false;
        image = GetComponent<Image>();
        image.color = new Color(1f, 1f, 1f, 0f);
        EventManager.m_Instance.AddListener<MapUpdateEvent>(IndicatePosition);
    }
    // Update is called once per frame
    void Update()
    {
        /*
        if (LevelNavigator.Instance.currentAreaID == ID)
        {
            image.color = new Color(1f, 1f, 1f);
        } else
        {
            image.color = new Color(0.2f, 0.2f, 0.2f);
        }
        */
    }

    public void IndicatePosition(MapUpdateEvent e)
    {
        Debug.Log("Map Udated!");
        float alpha = 0;
        if (LevelNavigator.Instance.currentAreaID == ID)
        {
            image.color = new Color(1f, 1f, 1f,1f); //Cambiar por la imagen del minimapa
            crossed = true;
        }
        else
        {
            if (crossed)
            {
                alpha = 1;
            } else
            {
                alpha = 0;
            }
                image.color = new Color(0.2f, 0.2f, 0.2f, alpha);
        }
    }

    public void LoadFromSave()
    {
        // El guardador de datos debería guardar si este tile se ha activado antes, para que cuando Drew Regrese, siga presente.
    }
}
