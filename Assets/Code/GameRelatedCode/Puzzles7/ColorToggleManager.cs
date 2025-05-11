using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


 public enum ColorType {Magenta, Cyan}

public enum PlatformGroup{PuzzleRoom, OtherRoom}

public class ColorToggleManager : MonoBehaviour
{
   public static ColorToggleManager Instance;

   private ColorType currentColor = ColorType.Magenta;
   public ColorType CurrentColor => currentColor;
   private bool puzzleCompleted = false;
   public bool PuzzleCompleted => puzzleCompleted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    public void SetActiveColor(ColorType newColor){
        if(puzzleCompleted || newColor == currentColor)  return;

        currentColor = newColor;
        Debug.Log($"Color cambiado a: {currentColor}");
    }

    public void CompletePuzzle(){
        if(puzzleCompleted) return; 

        puzzleCompleted = true;
        Debug.Log("Puzzle completado"); 
    }
}
