using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Panel del Menú de Pausa")]
    public GameObject pauseMenuPanel;

    [Header("Botones del Menú de Pausa")]
    public Button resumeButton;
    public Button settingsButton;
    public Button mainMenuButton;

    [Header("Panel de Opciones")]
    public GameObject settingsPanel;

    [Header("Botón Regresar en Opciones")]
    public Button regresarPausaButton; // Asigna este botón en el Inspector

    [Header("Referencias a InputHandler")]
    public InputHandler inputHandler;

    private bool isPaused = false;
    private bool previousMenuButtonState = false;

    void Start()
    {
        // Asegurarse de que el panel de pausa esté inicialmente desactivado
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("El Panel del Menú de Pausa no está asignado en el Inspector.");
        }

        if (inputHandler == null)
        {
            Debug.LogError("El InputHandler no está asignado en el Inspector.");
        }

        // Asignar listeners a los botones del menú de pausa
        if (resumeButton != null) resumeButton.onClick.AddListener(ResumeGame);
        if (settingsButton != null) settingsButton.onClick.AddListener(OpenSettings);
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(LoadMainMenu);

        // Asegurarse de que el panel de opciones esté inicialmente desactivado (si existe)
        if (settingsPanel != null) settingsPanel.SetActive(false);

        // Asignar listener al botón RegresarPausa si está asignado
        if (regresarPausaButton != null)
        {
            regresarPausaButton.onClick.AddListener(CloseSettingsMenu);
        }
    }

    void Update()
    {
        // Detectar la pulsación de la tecla de menú usando el bool del InputHandler
        if (inputHandler != null && inputHandler.pressPause)
        {
            // Asegurarse de que el botón se haya soltado para evitar toggles múltiples con una sola pulsación
            if (!previousMenuButtonState)
            {
                TogglePause();
                // Actualizar el estado anterior del botón
                previousMenuButtonState = true;
            }
        }
        else
        {
            // Si el botón no está presionado, resetear el estado anterior
            previousMenuButtonState = false;
        }
    }

    public void TogglePause()
    {
        if (pauseMenuPanel == null) return;

        isPaused = !isPaused;

        pauseMenuPanel.SetActive(isPaused);

        // Congelar o descongelar el tiempo del juego
        Time.timeScale = isPaused ? 0f : 1f;

        // Opcional: Mostrar/ocultar el cursor
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // O tu estado de bloqueo de cursor normal
            Cursor.visible = false;
        }
    }

    public void ResumeGame()
    {
        TogglePause();
    }

    public void OpenSettings()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    public void CloseSettingsMenu()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}