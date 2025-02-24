using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Mission
{
    public string title;
    public string description;
    public bool clear;

    public Image PendingImage;
    public Image ClearImage;

    private Image displayImage;

    public Mission(string Title, Image pending, Image cleared)
    {
        this.title = Title;
        clear = false;
        PendingImage = pending;
        ClearImage = cleared;
    }

    public void Clear()
    {
        this.clear = true;
        this.displayImage = ClearImage;
    }
}
