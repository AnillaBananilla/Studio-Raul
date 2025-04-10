using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelNavigator : MonoBehaviour
{
    public int currentAreaID; //ID del checkpoint en donde Drew se encuentra ahora.
    private static LevelNavigator instance;

    public static LevelNavigator Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<LevelNavigator>();
            return instance;
        }
    }

    public void UpdateMap(int ID)
    {
        currentAreaID = ID;
        //Debug.Log(currentAreaID);
    }

    public void GoToScene1()
    {
        SceneManager.LoadScene(1);
    }
    public void GoToScene0()
    {
        SceneManager.LoadScene(0);
    }
}
