using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static PlayerSkills;


public class GameManager : MonoBehaviour
{
    private static GameManager _instance;           // Asignar aca el unico game manager.
    //public TextMeshProUGUI scoreText;
    //public TextMeshProUGUI keyText;
    public Image lifeBar;
    public float healtAmount = 100f;
    public int score;
    public int key;

    [Header("UI Elements for GameOver")]
    [SerializeField] private GameObject gameOverPanel; // Referencia al panel de Game Over
    [SerializeField] private Button menuButton; // Botón para volver al menú
    [SerializeField] private Button restartButton; // Botón para reiniciar
    private bool isGameOver = false;


    public PlayerSkills SkillList;

    public float pintureAmount = 100f;
    public Image pintureBar;



    public Transform RespawnPoint;
    public bool Dead = false;
    public PlayerStats playerStats;


    /*
    public GameObject AchievementScreen;        //Pendiente por terminar
    public GameObject MissionScreen;            //Pendiente por terminar
    public GameObject InventoryScreen;          //Pendiente por terminar
    public GameObject ShopScreen;               //Pendiente por terminar
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
        float appliedDamage = damage - playerStats.currentDamageReduction;
        healtAmount -= appliedDamage;
        lifeBar.fillAmount = healtAmount / 100F;
        if(lifeBar.fillAmount == 0){
            TriggerGameOver();
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

    /*
    public void GoToAchievements()
    {
        MissionScreen.SetActive(false);
        AchievementScreen.SetActive(true);
        InventoryScreen.SetActive(false);
    }

    public void GoToMissions()
    {
        AchievementScreen.SetActive(false);
        InventoryScreen.SetActive(false);
        MissionScreen.SetActive(true);
    }

    public void GoToInventory()
    {
        MissionScreen.SetActive(false);
        AchievementScreen.SetActive(false);
        InventoryScreen.SetActive(true);
    }
    public void OpenShop()
    {
        ShopScreen.SetActive(true);
    }

    public void CloseMenu()
    {
        MissionScreen.SetActive(false);
        InventoryScreen.SetActive(false);
        AchievementScreen.SetActive(false);
        ShopScreen.SetActive(false);

    }
 
    public void EquipItem(EquipItemEvent e)
    {
        EquippedItem = e.eventItem;
    }
    */

    public void usePinture(float pinture)
    {
        pintureAmount -= pinture;
        pintureBar.fillAmount = pintureAmount / 100F;
    }
    public void recivePinture(float pinture)
    {
        pintureAmount += pinture;
        pintureBar.fillAmount = pintureAmount / 100F;
    }
    
    
    public void LoadScene()
    {
        SaveManager.OpenSavedScene();
    }

    public void LoadData()
    {
        PlayerData LoadedData = SaveManager.LoadPlayerData();
       
    }

    public void SaveData()
    {
        
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
