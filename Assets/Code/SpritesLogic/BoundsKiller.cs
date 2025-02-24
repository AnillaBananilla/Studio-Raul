using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsKiller : MonoBehaviour
{

    public GameObject Player;
    //public Transform RespawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            EventManager.m_Instance.InvokeEvent<DieEvent>(new DieEvent());
        }
    }

}
