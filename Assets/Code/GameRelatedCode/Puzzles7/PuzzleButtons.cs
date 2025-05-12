using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButtons : MonoBehaviour
{
    public ColorToggleManager manager;
    public ColorType buttonColor;

    public void toggleColor(){
        manager.ChangeCurrColor(buttonColor);
        Debug.Log($"El color del botón actual es {buttonColor}");
    }
}
