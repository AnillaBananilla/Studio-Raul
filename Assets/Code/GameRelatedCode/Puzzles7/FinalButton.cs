using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalButton : MonoBehaviour
{
    private bool triggered = false;

    public void Activate(){
        if(triggered) return;
        triggered = true;

        Debug.Log("Final button triggered!");
        ColorToggleManager.Instance.CompletePuzzle();
    }
}
