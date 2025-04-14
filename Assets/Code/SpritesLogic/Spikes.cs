using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Spikes : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager gameManagerX;


    private int pointValue;
    void Start()
    {
        gameManagerX = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
     
        if (other.gameObject.CompareTag("Player"))
        {
            gameManagerX.takeDamage(20); //Corregir para que le pegue a healt
        }
    }
  
}
