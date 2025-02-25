using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World1_Manager : MonoBehaviour
{
    private static World1_Manager instance;

    public static World1_Manager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<World1_Manager>();
            }
                
            return instance;
        }
       
    }

    private char _color = 'r';

    public char CurrentColor()
    {
        return _color;
    }

    public void ColorUpdate(char C)
    {
        _color = C;
        Debug.Log(_color);
    }
}
