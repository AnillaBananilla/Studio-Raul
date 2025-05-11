using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorToggleManager : MonoBehaviour
{

    public enum ColorType{
        Magenta, 
        Cyan
    }

    public enum PlatformGroup{
        PuzzleRoom,
        OtherRoom
    }

    public static ColorToggleManager Instance;

    public static event Action<ColorType> OnColorChanged;
    public static event Action OnClearPlatforms;

    private ColorType currentColor;
    private bool puzzleCompleted = false;

    //singleton de este color toggle manager
    private void Awake()
    {
        if(Instance != null) Destroy(gameObject);
        else Instance = this;   
    }
    
    public void CompletePuzzle(){
        if(puzzleCompleted) return;

        puzzleCompleted = true;
        OnClearPlatforms?.Invoke();
        OnColorChanged?.Invoke(ColorType.Cyan);
    }
}
