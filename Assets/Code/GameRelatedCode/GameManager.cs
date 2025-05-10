using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static PlayerSkills;
using Unity.VisualScripting;
using System;
using Unity.VisualScripting.Dependencies.Sqlite;


public class GameManager : MonoBehaviour
{
    private static GameManager _instance;           // Asignar aca el unico game manager.
    //public TextMeshProUGUI scoreText;
    //public TextMeshProUGUI keyText;
    public Image lifeBar;
    public Sprite fullLife;
    public Sprite mediumLife;
    public Sprite lowLife;
    public Sprite noLife;
    public Animator animatorHeart;
    public float healtAmount = 100f;
    public int score;
    public int key;

    public PlayerInventory Inventory;
    [Header("UI Elements for GameOver")]
    [SerializeField] private GameObject gameOverPanel; // Referencia al panel de Game Over
    [SerializeField] private Button menuButton; // Botón para volver al menú
    [SerializeField] private Button restartButton; // Botón para reiniciar

    private bool isGameOver = false;


    public PlayerSkills SkillList;

    //public float pintureAmount = 100f;
    public float[] paintAmount = { 100f, 100f, 100f }; // Az Ma Am
    public int paintColorIndex = 0;

    public Image pintureBar;



    public Transform RespawnPoint;
    public bool Dead = false;
    public Healt PlayerHP;
    public PlayerStats playerStats;

    [Header("CANVAS DE GUARDADO")]
    public InputHandler PlayerInput;
    private float textboxspeed = 0f;
    [SerializeField] private bool inDialog = false;


    private Animator _saveanimator;

    [Header("Texto Del Dialogo")]
    public TextMeshProUGUI Dialog;

    [Header("Efecto Typewriter")]
    public TypewriterEffect effect;


    [Header("Parent del Dialogo De guardado")]
    public GameObject SaveDialog;

    [Header("Botones")]
    public Button YesButton; public Button NoButton; public Button OkButton;

    public Image redOverlay;

    public void TintScreenRed()
    {
        if (redOverlay != null)
        {
            redOverlay.gameObject.SetActive(true);
            redOverlay.color = new Color(1, 0, 0, 0.5f); 
        }
    }


    /*
    TO DO:
    A�adir una lista de booleanos, o lo que sea, que indiquen el progreso del juego.
    */

    public float fadeInDuration = 1.5f;
    public float fadeOutDuration = 1.5f;

    private float fadeSpeed;

    public Item EquippedItem;

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>(); //Intenta buscar uno
            }
            return _instance; //Ahora tengo un singleton.
        }
    }

    void Start()
    {
        Dead = false;
        score = 0;
        UpdateScore(0);
        key = 0;
        UpdateKey(0);
        fadeSpeed = 1f / fadeInDuration;
        //EventManager.m_Instance.AddListener<EquipItemEvent>(EquipItem);


        // Asegurarse de que el panel esté desactivado al inicio
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Asignar funciones a los botones
        if (menuButton != null)
            menuButton.onClick.AddListener(GoToMenu);
        
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        if (YesButton != null) YesButton.onClick.AddListener(SaveGame);
        if (NoButton != null) NoButton.onClick.AddListener(EndDialog);
        if (OkButton != null) OkButton.onClick.AddListener(EndDialog);

        _saveanimator = SaveDialog.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        //scoreText.text = "score: " + score;
       
    }
    public void UpdateKey(int keyToAdd)
    {
        key += keyToAdd;
        Debug.Log("keys: " + key);

    }
    public bool HasKey()
    {
        return key > 0;
    }

    public void takeDamage(float damage)
    {
        //ACOMODAR PARA QUE USE EL COMPONENTE HEALT DE PLAYER

        
        float appliedDamage = Mathf.Max(0, damage - playerStats.currentDamageReduction);
        PlayerHP.currentHealt -= (int) appliedDamage;
        Debug.Log("Recibes" + appliedDamage);
        lifeBar.fillAmount = PlayerHP.currentHealt / (float)PlayerHP.maxHealt;
        animatorHeart.SetTrigger("DamageHeart");
        switch (PlayerHP.currentHealt)
        {
            case int n when (n > 75 && n <= 100):
                lifeBar.sprite = fullLife;
                break;
            case int n when (n > 50 && n <= 75):
                lifeBar.sprite = mediumLife;
                break;
            case int n when (n > 25 && n <= 50):
                lifeBar.sprite = lowLife; 
                break;
            case int n when (n >= 0 && n <= 25):
                lifeBar.sprite = noLife;
                break;
            default:
                Debug.LogWarning("Health value out of expected range.");
                break;
        }
        if (lifeBar.fillAmount == 0){
            TriggerGameOver();
            Dead = true;
        }
    }
    IEnumerator FadeIn(CanvasGroup canvasGroup, float waitTime)
    {
        yield return new WaitForSeconds(waitTime); 

        float currentAlpha = 0f;
        float fadeSpeed = 1f / fadeInDuration;

        while (currentAlpha < 1f)
        {
            currentAlpha += fadeSpeed * Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(currentAlpha);
            yield return null;
        }
    }
    IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        yield return new WaitForSeconds(5);

        float currentAlpha = 1f;
        float fadeSpeed = 1f / fadeOutDuration;

        while (currentAlpha > 0f)
        {
            currentAlpha -= fadeSpeed * Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(currentAlpha);
            yield return null;
        }
    }

    public bool BrushSkill()
    {
        Skill pincelSkill = SkillList.skills.Find(skill => skill.name == "Pincel");
        return pincelSkill.isUnlocked;
    }

    public void usePinture(float pinture)
    {
        //pintureAmount -= pinture;

        paintAmount[paintColorIndex] -= pinture;

        //pintureBar.fillAmount = pintureAmount / 100F;

        pintureBar.fillAmount = paintAmount[paintColorIndex]/100f;
    }
    public void recivePinture(float pinture)
    {
        //pintureAmount += pinture;
        //pintureBar.fillAmount = pintureAmount / 100F;

        for (int i = 0; i>3; i++)
        {
            if (paintAmount[i] + pinture <= 100)
            {
                paintAmount[i] += pinture;
            } 
        }
        pintureBar.fillAmount = paintAmount[paintColorIndex]/100f;
    }
    private void EndDialog()
    {
        PlayerInput.moveable = true;
        Time.timeScale = 1f;
        SaveData();
        paintAmount[0] = 100f;
        paintAmount[1] = 100f;
        paintAmount[2] = 100f;
        recivePinture(0);
        StartCoroutine(ResetDialog());
        //LoadData();
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
    public void OpenDialog()
    {
        if (!inDialog)
        {
            _saveanimator.SetTrigger("OpenDialog");
            if (effect != null)
            {
                effect.StartTypewriter("¿Te gustaría guardar tu partida?");
            }
            inDialog = true;
            PlayerInput.moveable = false;
            Time.timeScale = 0f;
        }
        
    }

    public void Freeze()
    {
        PlayerInput.moveable = false;
    }
    public void Melt()
    {
        PlayerInput.moveable = true;
    }


    public void LoadScene()
    {
        
    }

    public void LoadData()
    {
        PlayerData Save = SaveManager.LoadPlayerData();
        //Cargar los datos del jugador:
        PlayerHP.gameObject.transform.position = new Vector3(Save.position[0], Save.position[1], -1.2563f); //Position
        score = Save.Money;
        PlayerHP.currentHealt = Save.HP;
        key = Save.Keys;
        //Cargar los items
        int i = 0;
        foreach (int amount in Save.ItemAmounts)
        {
            Inventory.items[i].quantity = amount;
            i++;
        }
        //Cargar los Skills
        SkillList.skills[0].isUnlocked = Save.Skills[0];
        SkillList.skills[1].isUnlocked = Save.Skills[1];
        SkillList.skills[2].isUnlocked = Save.Skills[2];
        SkillList.skills[3].isUnlocked = Save.Skills[3];
        usePinture(0);

       //To Do:
       //Cargar los datos de misiones terminadas e items almacenados.
    }

    public void SaveData()
    {
        SaveManager.SavePlayerData(instance);
        //SaveManager.SavePlayerData(Drew);
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        
        // Activar panel de Game Over
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        
        // Detener el tiempo
        Time.timeScale = 0f;
    }

    private void GoToMenu()
    {
        // Restaurar el tiempo
        Time.timeScale = 1f;
        
        // Cambiar a la escena del menú (ajusta "MenuScene" al nombre de tu escena)
        SceneManager.LoadScene("MainMenu");
    }

    private void RestartGame()
    {
        isGameOver = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Time.timeScale = 1f;

        PlayerInput.moveable = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
