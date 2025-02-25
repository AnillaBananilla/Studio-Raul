using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGBCanvas : MonoBehaviour
{
    
   private void OnCollisionEnter2D(Collision2D other)
    {
        switch (World1_Manager.Instance.CurrentColor())
        {
            case 'r':
                World1_Manager.Instance.ColorUpdate('g');
                break;
            case 'g':
                World1_Manager.Instance.ColorUpdate('b');
                break;
            case 'b':
                World1_Manager.Instance.ColorUpdate('r');
                break;
            default:
                World1_Manager.Instance.ColorUpdate('r');
                break;
        }
    }
}
