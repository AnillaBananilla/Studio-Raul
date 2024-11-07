using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScreen : MonoBehaviour
{
    public Button ItemSlot_1, ItemSlot_2, ItemSlot_3;
    public Button Equipped_1, Equipped_2, Equipped_3;

    private int equipped_index = 0;

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    void Update()
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
