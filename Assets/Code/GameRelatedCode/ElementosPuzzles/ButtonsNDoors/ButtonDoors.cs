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

    [Header("Color Puzzle")]
    public bool isColorButton = false;
    public ColorType buttonColor;

    
    public void Activate(){
        if(!isPressed){
            isPressed = true;
            if (!moved)
            {
                transform.position += pressedOffset;
                moved = true;
            }
            buttonManager.CheckPuzzleState();

            if(isColorButton){
                Debug.Log($"Color button pressed: {buttonColor}");
                ColorToggleManager.Instance.SetActiveColor(buttonColor);
            }
        }
    }
}
