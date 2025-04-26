using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMisiones : MonoBehaviour
{
    public GameObject panel;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AlternarPanel();
        }
    }
    public void AlternarPanel()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
