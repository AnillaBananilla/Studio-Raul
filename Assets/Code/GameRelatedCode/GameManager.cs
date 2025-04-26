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
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI keyText;
    public Image lifeBar;
    public float healtAmount = 100f;
    public int score;
    public int key;

    public PlayerInventory Inventory;

    public PlayerSkills SkillList;

    public float pintureAmount = 100f;
    public float[] paintAmount = { 100f, 100f, 100f }; // Az Ma Am
    public int paintColorIndex = 0;

    public Image pintureBar;



    public Transform RespawnPoint;
    public bool Dead = false;
    public Healt PlayerHP;
    public PlayerStats playerStats;


    /*
    TO DO:
    Añadir una lista de booleanos, o lo que sea, que indiquen el progreso del juego.
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
        pintureAmount -= pinture;

        paintAmount[paintColorIndex] -= pinture;

        pintureBar.fillAmount = pintureAmount / 100F;

        pintureBar.fillAmount = paintAmount[paintColorIndex];
    }
    public void recivePinture(float pinture)
    {
        pintureAmount += pinture;
        pintureBar.fillAmount = pintureAmount / 100F;
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
        usePinture(0);

       //To Do:
       //Cargar los datos de misiones terminadas e items almacenados.
    }

    public void SaveData()
    {
        SaveManager.SavePlayerData(instance);
        //SaveManager.SavePlayerData(Drew);
    }
}
