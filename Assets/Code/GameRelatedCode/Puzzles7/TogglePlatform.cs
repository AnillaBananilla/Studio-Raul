using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class TogglePlatform : MonoBehaviour
{
    public ColorType platformColor;
    public PlatformGroup platformGroup;

    private void OnEnable(){
        ColorToggleManager.OnColorChanged += HandleColorChange;
        ColorToggleManager.OnClearPlatforms += HandleClear;
    }

    private void OnDisable()
    {
        ColorToggleManager.OnColorChanged -= HandleColorChange;
        ColorToggleManager.OnClearPlatforms -= HandleClear;
    }

    private void HandleColorChange(ColorType activeColor){
        if(platformGroup == PlatformGroup.PuzzleRoom){
            gameObject.SetActive(platformColor == activeColor);
        }
        else if(platformGroup == PlatformGroup.OtherRoom){
            gameObject.SetActive(platformColor == ColorType.Cyan);
        }
    }

    private void HandleClear(){
        if(platformGroup == PlatformGroup.PuzzleRoom){
            gameObject.SetActive(false);
        }
    }


}
