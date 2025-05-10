using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnInteractDialog : MonoBehaviour
{
    public DialogueLine[] dialogueLines;

    public void TriggerCutscene()
    {
        CutsceneSystem.Instance.StartCutscene(dialogueLines);
        this.gameObject.SetActive(false);
    }

    void Start()
    {
        //TriggerCutscene();
    }

    public void OnDestroy()
    {
        TriggerCutscene();   
    }
}
