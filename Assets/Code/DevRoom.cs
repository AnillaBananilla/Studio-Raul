using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevRoom : MonoBehaviour
{
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.isGameActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
