using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsScreen : MonoBehaviour
{

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void GotoInventory()
    {
        GameManager.instance.GoToInventory();
    }

    public void GotoMissions()
    {
        GameManager.instance.GoToMissions();
    }

    public void Close()
    {
        GameManager.instance.CloseMenu();
    }
}
