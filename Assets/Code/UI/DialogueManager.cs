using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dBox;
    public TextMeshProUGUI dText;

    public bool dialogActive;

    [Header("Dialogue Lines")]
    public string[] dialogLines;
    public int currentLine;



    void Start()
    {

        dBox.SetActive(false);
    }

    void Update()
    {
        if (dialogActive && Input.GetKeyDown(KeyCode.Space))
        {
            currentLine++;
        }
        if (currentLine >= dialogLines.Length)
        {
            dBox.SetActive(false);
            dialogActive = false;

            currentLine = 0;

        }
        dText.text = dialogLines[currentLine];
    }

    public void ShowDialogue()
    {
        dialogActive = true;
        dBox.SetActive(true);
  
    }
}
