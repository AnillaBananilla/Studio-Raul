using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayButton : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject uiGame;
    private Button playButton;
    public GameManager gameManager;
    public GameObject pailLogo;
    public GameObject introScreen;
    // Start is called before the first frame update
    void Start()
    {
        playButton = GetComponent<Button>();
        playButton.onClick.AddListener(PlayGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PlayGame()
    {
        gameManager.isGameActive = true;
        titleScreen.SetActive(false);
        pailLogo.SetActive(false);
        titleScreen.SetActive(false);
        introScreen.SetActive(false);
        uiGame.SetActive(true);
    }
    public void QuitGame()
    {
    
        Application.Quit();
    }
}
