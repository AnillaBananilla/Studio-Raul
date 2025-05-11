using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalButton : MonoBehaviour
{
    private bool triggered = false;

    public void Trigger()
    {
        if (triggered) return;
        triggered = true;

        ColorToggleManager.Instance.CompletePuzzle();
    }
}
