using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static PlayerSkills;
using Unity.VisualScripting;


public class GameManager : MonoBehaviour
{
    private static GameManager _instance;           // Asignar aca el unico game manager.
    //public TextMeshProUGUI scoreText;
    //public TextMeshProUGUI keyText;
    public Image lifeBar;
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
        float appliedDamage = damage - playerStats.currentDamageReduction;
        PlayerHP.currentHealt -= (int) appliedDamage;
        lifeBar.fillAmount = PlayerHP.currentHealt / 100F;
        if(lifeBar.fillAmount == 0){
            TriggerGameOver();
            Dead = true;
        }
        lifeBar.fillAmount = PlayerHP.currentHealt / 100F;
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
        
        // Desactivar panel de Game Over
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        // Restaurar el tiempo
        Time.timeScale = 1f;
        
        // Recargar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
