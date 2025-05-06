using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    
    public ButtonDoors[] buttons;
    public GameObject closedDoor;
    public GameObject openDoor;

    public void CheckPuzzleState(){
        foreach(var button in buttons){
            if(!button.isPressed){
                return;
            }
        }
        Debug.Log("Ambos botones se presionaron");
        OpenDoor();
    }
   
   public void OpenDoor(){
        closedDoor.SetActive(false);
        openDoor.SetActive(true);
   }
}
