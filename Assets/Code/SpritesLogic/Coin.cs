using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Coins : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager gameManagerX;


    public int pointValue;
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
            gameManagerX.UpdateScore(pointValue);
       Destroy(gameObject);
        }
    }
  
}
