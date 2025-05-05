using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class SaveFountain : MonoBehaviour
{
    public InputHandler Player;
    private bool _interact = false;
    // Start is called before the first frame update
    void Start()
    {

        _interact = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Player != null) && Player.interacting)
        {
            GameManager.instance.OpenDialog();
        }
        
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _interact = true;
            
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _interact = false;
        }
    }

    


}
