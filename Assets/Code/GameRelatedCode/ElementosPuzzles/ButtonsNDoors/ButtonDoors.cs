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
    public FinalButton isFinalButton;

    
    public void Activate(){
        if(!isPressed && !isColorButton){
            isPressed = true;
            if (!moved)
            {
                transform.position += pressedOffset;
                moved = true;
            }
            buttonManager.CheckPuzzleState();
        }

        else if(!isPressed && isColorButton){
                Debug.Log($"Color button pressed: {buttonColor}");
                ColorToggleManager.Instance.SetActiveColor(buttonColor);
        }
        else if(!isPressed && isFinalButton != null){
            isFinalButton.Trigger();
            return;
        }
    }
}
