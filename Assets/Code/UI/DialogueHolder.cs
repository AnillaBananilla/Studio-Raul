using UnityEngine;

public class DialogueHolder : MonoBehaviour
{
    private DialogueManager DM;

    public string[] dialogueLines;

    void Start()
    {
        DM = FindFirstObjectByType<DialogueManager>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetKey(KeyCode.E))
        {
            if (!DM.dialogActive)
            {
                if (TryGetComponent(out QuestObject quest) && !quest.isAccepted)
                {
                    quest.OfferQuest();
                }
                else
                {
                    DM.dialogLines = dialogueLines;
                    DM.currentLine = 0;
                    DM.ShowDialogue();
                }
            }
        }
    }

}
