using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log(currentAreaID);
    }
}
