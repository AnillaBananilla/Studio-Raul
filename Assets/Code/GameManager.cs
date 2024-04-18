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

    void Start()
    {
        score = 0;
        UpdateScore(0);
  

    }

    // Update is called once per frame
    void Update()
    {
        
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

}
