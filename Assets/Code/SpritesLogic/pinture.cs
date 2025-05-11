using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinture : MonoBehaviour
{
    public GameManager gameManagerX;


    public int pointValue;
    public int color;
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
            gameManagerX.paintAmount[color] = 20f; ;
            /*if (gameManagerX.pintureAmount < 100)
            {
                gameManagerX.recivePinture(20);
            }*/

            Destroy(gameObject);
        }
    }
}
