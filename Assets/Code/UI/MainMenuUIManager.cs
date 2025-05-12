using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("Paneles del Menú")]
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

        // Lógica adicional para el panel principal (vacía)
    }

    public void ShowNewGamePanel()
    {
        DeactivateAllPanels();
        newGamePanel.SetActive(true);

        // Lógica adicional para el panel de nuevo juego (vacía)
    }

    public void ShowLoadGamePanel()
    {
        DeactivateAllPanels();
        loadGamePanel.SetActive(true);

        // Lógica adicional para el panel de cargar juego (vacía)
    }

    public void ShowSettingsPanel()
    {
        DeactivateAllPanels();
        settingsPanel.SetActive(true);

        // Lógica adicional para el panel de configuración (vacía)
    }

    private void DeactivateAllPanels()
    {
        mainPanel.SetActive(false);
        newGamePanel.SetActive(false);
        loadGamePanel.SetActive(false);
        settingsPanel.SetActive(false);
    }
}