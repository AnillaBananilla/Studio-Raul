using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivator : MonoBehaviour
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


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerCutscene();
        }
    }

   
}

