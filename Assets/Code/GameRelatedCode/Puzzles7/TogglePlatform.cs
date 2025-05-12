using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class TogglePlatform : MonoBehaviour
{
    public ColorType platformColor;
    public PlatformGroup platformGroup;
    public bool childActive;

    public void ActivateChildren(){
        foreach(Transform child in gameObject.transform){
            child.gameObject.SetActive(true);
        }
        childActive = true;
    }
    public void DeactivateChildren(){
        foreach(Transform child in gameObject.transform){
            child.gameObject.SetActive(false);
        }
        childActive = false;
    }

}
