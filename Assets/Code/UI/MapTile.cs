using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTile : MonoBehaviour
{
    public int ID;

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
        image.color = new Color(1f, 1f, 1f);
    }
    // Update is called once per frame
    void Update()
    {
        if (LevelNavigator.Instance.currentAreaID == ID)
        {
            image.color = new Color(1f, 1f, 1f);
        } else
        {
            image.color = new Color(0.2f, 0.2f, 0.2f);
        }
    }
}
