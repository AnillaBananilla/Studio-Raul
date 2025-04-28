using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class SaveFountain : MonoBehaviour
{
    private InputHandler Player;
    private float textboxspeed = 0f;
    [SerializeField] private bool inDialog = false;

 
    private Animator _animator;

    [Header("Texto Del Dialogo")]
    public TextMeshProUGUI Dialog;

    [Header("Efecto Typewriter")]
    public TypewriterEffect effect;


    [Header("Parent del Dialogo")]
    public GameObject SaveDialog;

    [Header("Botones")]
    public Button YesButton; public Button NoButton; public Button OkButton;


    // Start is called before the first frame update
    void Start()
    {
        Player = null;
        if (YesButton != null) YesButton.onClick.AddListener(SaveGame);
        if (NoButton != null) NoButton.onClick.AddListener(EndDialog);
        if (OkButton != null) OkButton.onClick.AddListener(EndDialog);

        _animator = SaveDialog.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if ((Player != null))
        {
            if (Player.interacting && !inDialog)
            {
                //Open the Save Game Option.
                Player.moveable = false;
                OpenDialog();
                inDialog = true;
                Time.timeScale = 0f;
                Debug.Log("The Dialog Opens");
            }
        }
        
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player = collision.gameObject.GetComponent<InputHandler>();
            Debug.Log("Player Found!");
            Debug.Log(Player);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player = null;
            inDialog = false;
        }
    }

    private void OpenDialog()
    {
        _animator.SetTrigger("OpenDialog");
        if (effect != null)
        {
            effect.StartTypewriter("¿Te gustaría guardar tu partida?");
        }

    }

    private void EndDialog()
    {
        Player.moveable = true;
        Time.timeScale = 1f;
        StartCoroutine(ResetDialog());
        GameManager.instance.LoadData();
    }

    private void SaveGame()
    {
        YesButton.gameObject.SetActive(false);
        //Dialog.text = "Guardando..."; *Viejo
        if (effect != null)
        {
            effect.StartTypewriter("Guardando...");
        }
        NoButton.gameObject.SetActive(false);
        GameManager.instance.SaveData();
        GameManager.instance.pintureAmount = 100f;
        StartCoroutine(SaveBuffer());
    }

    private IEnumerator SaveBuffer()
    {
        yield return new WaitForSecondsRealtime(3);
        if (effect != null)
        {
            effect.StartTypewriter("Partida guardada con éxito.");
        }
        OkButton.gameObject.SetActive(true);
    }

    private IEnumerator ResetDialog()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        inDialog = false;
        YesButton.gameObject.SetActive(true);
        NoButton.gameObject.SetActive(true);
        OkButton.gameObject.SetActive(false);
    }
}
