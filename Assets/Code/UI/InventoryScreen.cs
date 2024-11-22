using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScreen : MonoBehaviour
{

    public List<GameObject> Elements = new List<GameObject>();


    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void EquipItem()
    {
        
    }

    public void GotoMissions()
    {
        GameManager.instance.GoToMissions();
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
