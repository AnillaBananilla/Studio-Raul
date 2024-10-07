using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI scoreText;
    public Image lifeBar;
    public float healtAmount = 100f;
    private int score;
<<<<<<< Updated upstream
    public bool isGameActive = false;
=======
    public bool isGameActive = false; // Default: false

>>>>>>> Stashed changes
    public CanvasGroup canvasGroupLogo;
    public CanvasGroup canvasGroupLogoColored;
    public CanvasGroup canvasGroupLogoMenu;

    public GameObject AchievementScreen;
    public GameObject MissionScreen;
    public GameObject InventoryScreen;

    public float fadeInDuration = 1.5f;
    public float fadeOutDuration = 1.5f;

    private float fadeSpeed;

    void Start()
    {
        score = 0;
        UpdateScore(0);
        fadeSpeed = 1f / fadeInDuration;
       // StartCoroutine(FadeIn(canvasGroupLogo, 2));
      //  StartCoroutine(FadeOut(canvasGroupLogo));
       // StartCoroutine(FadeIn(canvasGroupLogoColored, 8));
        StartCoroutine(FadeIn(canvasGroupLogoMenu, 0f));

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

    public void CloseMenu()
    {
        MissionScreen.SetActive(false);
        InventoryScreen.SetActive(false);
        AchievementScreen.SetActive(false);
    }
>>>>>>> Stashed changes
}
