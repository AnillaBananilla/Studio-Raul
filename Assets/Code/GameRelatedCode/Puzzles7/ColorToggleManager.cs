using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


 public enum ColorType
{
    Magenta,
    Cyan
}

public enum PlatformGroup
{
    PuzzleRoom,
    OtherRoom
}

public class ColorToggleManager : MonoBehaviour
{
   public static ColorToggleManager Instance;

   public static event Action<ColorType> OnColorChanged;
   public static event Action OnClearPlatforms;

   private ColorType currentColor;
   private bool puzzleCompleted = false;

    private void Awake()
    {
        if(Instance != null) Destroy(gameObject);
        else Instance = this;   
    }

    public void SetActiveColor(ColorType newColor){
        if(puzzleCompleted || newColor == currentColor) return;

        currentColor = newColor;
        OnColorChanged?.Invoke(currentColor);
    }

    public void CompletePuzzle(){
        if(puzzleCompleted){return;} 

        puzzleCompleted = true;
        //Disable puzzle room platforms
        OnClearPlatforms?.Invoke();
        //activate the room 7 above cyan platforms
        OnColorChanged?.Invoke(ColorType.Cyan); 
    }
}
