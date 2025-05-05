using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinture : MonoBehaviour
{
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
            gameManagerX.recivePinture(20);
            /*if (gameManagerX.pintureAmount < 100)
            {
                gameManagerX.recivePinture(20);
            }*/

            Destroy(gameObject);
        }
    }
}
