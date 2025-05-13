using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionFinalScene : MonoBehaviour
{
    public static TransitionFinalScene instance;
    public Animator transitionAnim1;
    public Animator transitionAnim2;


    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SceneFinal()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        // Disparar animaci�n FadeIn (Start)
        transitionAnim1.SetTrigger("Start");

        // Esperar el tiempo de duraci�n de la animaci�n FadeIn
        yield return new WaitForSeconds(3);

        // Disparar animaci�n FadeOut (End)
        transitionAnim2.SetTrigger("End");

        // Esperar el tiempo de duraci�n de la animaci�n FadeOut
        yield return new WaitForSeconds(2);

        // Cargar la siguiente escena
        SceneManager.LoadSceneAsync("Continuara");
    }
}
