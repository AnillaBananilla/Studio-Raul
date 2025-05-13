using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    public GameObject closedDoor;
    public GameObject openDoor;

    public void OpenDoor(){
        closedDoor.SetActive(false);
        openDoor.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            OpenDoor();
        }
    }
}
