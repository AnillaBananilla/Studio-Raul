using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager gameManagerX;


   public int keyValue;
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
            gameManagerX.UpdateKey(keyValue);
            Destroy(gameObject);
        }
    }
}
