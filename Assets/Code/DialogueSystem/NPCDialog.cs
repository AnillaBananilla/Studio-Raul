using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialog : MonoBehaviour
{
    // Start is called before the first frame update
    public DialogueLine[] dialogueLines;

    private bool inDialog = false;
    private InputHandler ih = null;
    public void TriggerCutscene()
    {
        CutsceneSystem.Instance.StartCutscene(dialogueLines);
    }

    void Start()
    {
        
    }
    public void Update()
    {
        if (ih != null && !inDialog)
        {
            if (ih.attack)
            {
                TriggerCutscene();
                inDialog = true;
            }
        }
        
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ih = other.GetComponent<InputHandler>();
        }

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ih = null;
            inDialog = false;
        }
    }
}