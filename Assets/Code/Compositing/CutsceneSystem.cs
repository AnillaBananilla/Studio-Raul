using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutsceneSystem : MonoBehaviour
{
    public static CutsceneSystem Instance;

    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public GameObject backgroundFade;

    public Image dialoguePanelImage;
    public Image leftPortrait, rightPortrait;
    public CanvasGroup leftGroup, rightGroup;

    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;

    [Header("Panel Styles")]
    public Sprite Texto_Pensamiento;
    public Sprite Texto_Neutral;
    public Sprite Texto_Melancolico;
    public Sprite Texto_Enojado;


    public Image dialoguePanelNameImage;
    public Sprite Nombre_Neutral;
    public Sprite Nombre_Melancolico;
    public Sprite Nombre_Enojado;

    private DialogueLine[] currentLines;
    private int lineIndex = 0;
    private bool isTyping = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
        }

        dialoguePanel.SetActive(false);
        backgroundFade.SetActive(false);
    }

    public void StartCutscene(DialogueLine[] lines)
    {
        currentLines = lines;
        lineIndex = 0;

        Time.timeScale = 0f; // pausa todo el juego

        dialoguePanel.SetActive(true);
        backgroundFade.SetActive(true);

        DisplayNextLine();
    }

    void ResumeWorld()
    {
        Time.timeScale = 1f;
    }

    void EndCutscene()
    {
        dialoguePanel.SetActive(false);
        backgroundFade.SetActive(false);
        ResumeWorld();
    }

    void Update()
    {
        if (!dialoguePanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = currentLines[lineIndex].lineText;
                isTyping = false;
            }
            else
            {
                lineIndex++;
                if (lineIndex >= currentLines.Length)
                {
                    EndCutscene();
                }
                else
                {
                    DisplayNextLine();
                }
            }
        }
    }

    void DisplayNextLine()
    {
        DialogueLine line = currentLines[lineIndex];
        DialogueCharacter character = line.character;
        Sprite portrait = character.GetPortrait(line.emotion);

        speakerText.text = character.characterName;
        dialogueText.text = "";

        if (character.isOnLeft)
        {
            leftPortrait.sprite = portrait;
            leftPortrait.gameObject.SetActive(true);
            rightPortrait.gameObject.SetActive(true);
            leftGroup.alpha = 1f;
            rightGroup.alpha = 0.4f;
        }
        else
        {
            rightPortrait.sprite = portrait;
            rightPortrait.gameObject.SetActive(true);
            leftPortrait.gameObject.SetActive(true);
            rightGroup.alpha = 1f;
            leftGroup.alpha = 0.4f;
        }

        switch (line.panelStyle.ToLower())
        {
            case "neutral":
                dialoguePanelImage.sprite = Texto_Neutral;
                break;
            case "enojado":
                dialoguePanelImage.sprite = Texto_Enojado;
                break;
            case "melancolico":
                dialoguePanelImage.sprite = Texto_Melancolico;
                break;
            case "pensamiento":
                dialoguePanelImage.sprite = Texto_Pensamiento;
                break;
            default:
                dialoguePanelImage.sprite = Texto_Neutral;
                break;
        }

        StartCoroutine(TypeLine(line.lineText));
    }

    IEnumerator TypeLine(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(0.02f); 
        }

        isTyping = false;
    }
}
