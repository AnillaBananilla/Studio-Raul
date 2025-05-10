using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ButtonDoors : MonoBehaviour
{
    public PuzzleManager buttonManager;
    public bool isPressed = false;
    
    public Vector3 pressedOffset = new Vector3(0, -0.5f, 0); // se hunde ligeramente
    private bool moved = false;
    public void Activate(){
        if(!isPressed){
            isPressed = true;
            if (!moved)
            {
                transform.position += pressedOffset;
                moved = true;
            }
            buttonManager.CheckPuzzleState();
        }
    }
}
