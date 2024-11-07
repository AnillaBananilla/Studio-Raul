using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionsScreen : MonoBehaviour
{

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void GotoInventory()
    {
        GameManager.instance.GoToInventory();
    }

    public void GotoAchievements()
    {
        GameManager.instance.GoToAchievements();
    }

    public void Close()
    {
        GameManager.instance.CloseMenu();
    }
}
