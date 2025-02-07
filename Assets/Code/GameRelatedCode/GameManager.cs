using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;           // Asignar aca el unico game manager.
    public TextMeshProUGUI scoreText;               // Dejemos esto pendiente.
    public Image lifeBar;
    public float healtAmount = 100f;
    public int score;
    private bool _RangeAttack = false;

    public bool isGameActive = false; // Default: false

    public Transform RespawnPoint;
    public bool Dead = false;



    public GameObject AchievementScreen;        //Pendiente por terminar
    public GameObject MissionScreen;            //Pendiente por terminar
    public GameObject InventoryScreen;          //Pendiente por terminar
    public GameObject ShopScreen;               //Pendiente por terminar

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
        fadeSpeed = 1f / fadeInDuration;




        EventManager.m_Instance.AddListener<EquipItemEvent>(EquipItem);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToInventory();
        }
    }
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "score: " + score;
       
    }
    public void takeDamage(float damage)
    {
       healtAmount -= damage;
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


    public void RangeUpgrade()
    {
        _RangeAttack = true;
        Debug.Log("NOW YOU CAN SHOOT");
    }

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

    public void Die()
    {
        
    }
}
