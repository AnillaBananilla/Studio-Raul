using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class TogglePlatform : MonoBehaviour
{
    public ColorType platformColor;
    public PlatformGroup platformGroup;

    private bool lastState = true;

    private void Start()
    {
        // Desactiva las plataformas cyan al iniciar el juego
        if (platformGroup == PlatformGroup.PuzzleRoom && platformColor == ColorType.Cyan)
        {
            gameObject.SetActive(false);
            lastState = false;
        }

        // También desactiva todas las plataformas del OtherRoom si el puzzle aún no está completo
        if (platformGroup == PlatformGroup.OtherRoom)
        {
            gameObject.SetActive(false);
            lastState = false;
        }
    }

    private void Update()
    {
        if (ColorToggleManager.Instance == null) return;
        bool shouldBeActive = false;
        
        if(platformGroup == PlatformGroup.PuzzleRoom){
            if(!ColorToggleManager.Instance.PuzzleCompleted){
                shouldBeActive = platformColor == ColorToggleManager.Instance.CurrentColor;
            }
        }
        else if(platformGroup == PlatformGroup.OtherRoom){
            if(ColorToggleManager.Instance.PuzzleCompleted){
                shouldBeActive = platformColor == ColorType.Cyan;
            }
        }

        if(shouldBeActive != lastState){
            gameObject.SetActive(shouldBeActive);
            lastState = shouldBeActive;
        }
    }


}
