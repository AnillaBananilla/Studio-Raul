using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CutsceneSystem : MonoBehaviour
{
    public static CutsceneSystem Instance;

    public GameObject DialogueCanvas;

    [Header("Retratos PNG")]
    public Image leftPortrait;
    public Image rightPortrait;
    public CanvasGroup leftGroup;
    public CanvasGroup rightGroup;

    [Header("Panel Izquierda")]
    public GameObject panelLeft;
    public Image panelLeftImage;
    public Image nameLeftImage;
    public TextMeshProUGUI speakerLeftText;
    public TextMeshProUGUI dialogueLeftText;

    [Header("Panel Derecha")]
    public GameObject panelRight;
    public Image panelRightImage;
    public Image nameRightImage;
    public TextMeshProUGUI speakerRightText;
    public TextMeshProUGUI dialogueRightText;

    [Header("Background y control")]
    public GameObject backgroundFade;

    [Header("Estilos de Panel de Diálogo")]
    public Sprite textoNeutral;
    public Sprite textoEnojado;
    public Sprite textoMelancolico;
    public Sprite textoPensamiento;

    [Header("Estilos de nombre")]
    public Sprite nombreNeutral;
    public Sprite nombreEnojado;
    public Sprite nombreMelancolico;

    [Header("UI ")]
    public GameObject NextButton;
    public CanvasGroup NextButtonGroup;



    private DialogueLine[] currentLines;
    private int lineIndex = 0;
    private bool isTyping = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        panelLeft.SetActive(false);
        panelRight.SetActive(false);
        backgroundFade.SetActive(false);
    }

    public void StartCutscene(DialogueLine[] lines)
    {
        currentLines = lines;
        lineIndex = 0;
        DialogueCanvas.SetActive(true);

        Time.timeScale = 0f; // Pausa el juego
        GameManager.instance.Freeze(); //Le quita el control al jugador
        backgroundFade.SetActive(true);
        DisplayNextLine();
    }

    void ResumeWorld()
    {
        Time.timeScale = 1f;
        GameManager.instance.Melt(); //Devuelve el control al jugador.
    }

    void EndCutscene()
    {
        panelLeft.SetActive(false);
        panelRight.SetActive(false);
        backgroundFade.SetActive(false);

        DialogueCanvas.SetActive(false);

        leftPortrait.gameObject.SetActive(false);  
        rightPortrait.gameObject.SetActive(false);  

        NextButton.SetActive(false);
        if (buttonBlink != null) StopCoroutine(buttonBlink);

        ResumeWorld();
    }



    void Update()
    {
        if (!panelLeft.activeSelf && !panelRight.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Debug.Log("Next");
            if (isTyping)
            {
                StopAllCoroutines();
                ShowFullText();
            }
            else
            {
                lineIndex++;
                if (lineIndex >= currentLines.Length)
                    EndCutscene();
                else
                    DisplayNextLine();
            }
        }
    }

    void ShowFullText()
    {
        DialogueLine line = currentLines[lineIndex];

        if (line.character.isOnLeft)
            dialogueLeftText.text = line.lineText;
        else
            dialogueRightText.text = line.lineText;

        isTyping = false;
    }

    void DisplayNextLine()
    {
        DialogueLine line = currentLines[lineIndex];
        DialogueCharacter character = line.character;
        Sprite portrait = character.GetPortrait(line.emotion);
        NextButton.SetActive(false);
        if (buttonBlink != null) StopCoroutine(buttonBlink);


        // Muestra retratos
        if (character.isOnLeft)
        {
            leftPortrait.sprite = portrait;
            leftGroup.alpha = 1f;
            rightGroup.alpha = 0.4f;
            Debug.Log(character.isOnLeft);

            panelLeft.SetActive(true);
            panelRight.SetActive(false);

            leftPortrait.gameObject.SetActive(true);
            rightPortrait.gameObject.SetActive(false);

            panelLeftImage.sprite = GetPanelSprite(line.panelStyle);
            bool isThinking = line.panelStyle.ToLower() == "pensamiento";
            bool showName = !isThinking && !string.IsNullOrWhiteSpace(character.characterName);
            nameLeftImage.gameObject.SetActive(showName);
            speakerLeftText.gameObject.SetActive(showName);

            if (showName)
            {
                nameLeftImage.sprite = GetNameSprite(line.panelStyle);
                speakerLeftText.text = character.characterName;
            }



            dialogueLeftText.text = "";

            StartCoroutine(TypeLine(dialogueLeftText, line.lineText));
        }
        else
        {
            rightPortrait.sprite = portrait;
            rightGroup.alpha = 1f;
            leftGroup.alpha = 0.4f;

            panelLeft.SetActive(false);
            panelRight.SetActive(true);

            leftPortrait.gameObject.SetActive(false);
            rightPortrait.gameObject.SetActive(true);

            panelRightImage.sprite = GetPanelSprite(line.panelStyle);
            bool isThinking = line.panelStyle.ToLower() == "pensamiento";

            bool showName = !isThinking && !string.IsNullOrWhiteSpace(character.characterName);
            nameRightImage.gameObject.SetActive(showName);
            speakerRightText.gameObject.SetActive(showName);

            if (showName)
            {
                nameRightImage.sprite = GetNameSprite(line.panelStyle);
                speakerRightText.text = character.characterName;
            }



            dialogueRightText.text = "";

            StartCoroutine(TypeLine(dialogueRightText, line.lineText));
        }
    }


    IEnumerator TypeLine(TextMeshProUGUI textBox, string text)
    {
        isTyping = true;
        NextButton.SetActive(false);
        NextButtonGroup.alpha = 0f;
        if (buttonBlink != null) StopCoroutine(buttonBlink);

        textBox.text = "";

        foreach (char c in text)
        {
            textBox.text += c;
            yield return new WaitForSecondsRealtime(0.02f);
        }

        isTyping = false;
        NextButton.SetActive(true);
        buttonBlink = StartCoroutine(BlinkNextButton());
    }




    Sprite GetPanelSprite(string style)
    {
        switch (style.ToLower())
        {
            case "neutral": return textoNeutral;
            case "enojado": return textoEnojado;
            case "melancolico": return textoMelancolico;
            case "pensamiento": return textoPensamiento;
            default: return textoNeutral;
        }
    }


    Sprite GetNameSprite(string style)
    {
        switch (style.ToLower())
        {
            case "neutral": return nombreNeutral;
            case "enojado": return nombreEnojado;
            case "melancolico": return nombreMelancolico;
            default: return nombreNeutral;
        }
    }

    Coroutine buttonBlink;

    IEnumerator BlinkNextButton()
    {
        float t = 0f;
        bool fadeOut = false;

        while (true)
        {
            t += Time.unscaledDeltaTime * 2f;

            float alpha = fadeOut ? 1f - t : t;
            NextButtonGroup.alpha = Mathf.Clamp01(alpha);

            if (t >= 1f)
            {
                t = 0f;
                fadeOut = !fadeOut;
            }

            yield return null;
        }
    }



}
