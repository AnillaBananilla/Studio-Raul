using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CinematicView: MonoBehaviour
{
    public VideoPlayer videoPlayer;     // Referencia al VideoPlayer
    public RawImage rawImage;          // Referencia al RawImage donde se muestra el video
    public string nextSceneName;       // Nombre de la escena a la que quieres ir
    public Button skipButton;          // Botón para saltar el video (opcional)

    void Start()
    {
        Debug.Log("Start CinematicView");
        // Asegurarse de que el video no esté reproduciendo aún
        if (videoPlayer != null)
        {
            // Suscribirse al evento de fin de video
            videoPlayer.loopPointReached += OnVideoFinished;

            // Si usas un botón para saltar
            if (skipButton != null)
            {
                skipButton.onClick.AddListener(SkipVideo);
            }
        }
    }

    // Método que se llama cuando el video termina
    private void OnVideoFinished(VideoPlayer vp)
    {
        // Cargar la siguiente escena
        SceneManager.LoadScene(nextSceneName);
    }

    // Método para saltar el video manualmente
    private void SkipVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop(); // Para el video
            SceneManager.LoadScene(nextSceneName); // Carga la siguiente escena
        }
    }

    // Limpieza de eventos al destruir el objeto
    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
        if (skipButton != null)
        {
            skipButton.onClick.RemoveListener(SkipVideo);
        }
    }
}