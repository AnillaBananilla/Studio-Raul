using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("Paneles del Men�")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject newGamePanel;
    [SerializeField] private GameObject loadGamePanel;
    [SerializeField] private GameObject settingsPanel;
    
    private void Start()
    {
        ShowMainPanel();
    }

    public void ShowMainPanel()
    {
        DeactivateAllPanels();
        mainPanel.SetActive(true);

        // L�gica adicional para el panel principal (vac�a)
    }

    public void ShowNewGamePanel()
    {
        DeactivateAllPanels();
        newGamePanel.SetActive(true);

        // L�gica adicional para el panel de nuevo juego (vac�a)
    }

    public void ShowLoadGamePanel()
    {
        DeactivateAllPanels();
        loadGamePanel.SetActive(true);

        // L�gica adicional para el panel de cargar juego (vac�a)
    }

    public void ShowSettingsPanel()
    {
        DeactivateAllPanels();
        settingsPanel.SetActive(true);

        // L�gica adicional para el panel de configuraci�n (vac�a)
    }

    private void DeactivateAllPanels()
    {
        mainPanel.SetActive(false);
        newGamePanel.SetActive(false);
        loadGamePanel.SetActive(false);
        settingsPanel.SetActive(false);
    }
}