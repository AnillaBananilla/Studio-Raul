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
    public ColorType currentColor;
    public TogglePlatform [] cyanPlatforms;
    public TogglePlatform [] magentaPlatforms;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        currentColor = ColorType.Magenta;
        UpdatePlatforms();
    }

    public void ChangeCurrColor(ColorType buttonColor){
        
        if(buttonColor == ColorType.Magenta){
            Debug.Log("Cambiando color actual a cyan...");
            currentColor = ColorType.Cyan;
            UpdatePlatforms();
        }
        else if(buttonColor == ColorType.Cyan){
            currentColor = ColorType.Magenta;
            UpdatePlatforms();
        }
    }

    public void UpdatePlatforms(){
        if(currentColor == ColorType.Magenta){
            foreach(TogglePlatform platform in magentaPlatforms){
                Debug.Log("Plataforma magenta activada");
                platform.ActivateChildren();
            }
            foreach(TogglePlatform platform in cyanPlatforms){
                Debug.Log("Plataforma cyan desactivada");
                platform.DeactivateChildren();
            }
        }
        else if(currentColor == ColorType.Cyan){
            foreach(TogglePlatform platform in cyanPlatforms){
                Debug.Log("Plataforma cyan activada");
                platform.ActivateChildren();
            }
            foreach(TogglePlatform platform in magentaPlatforms){
                Debug.Log("Plataforma magenta desactivada");
                platform.DeactivateChildren();
            }
        }
    }
}
