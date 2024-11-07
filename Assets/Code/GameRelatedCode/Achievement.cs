using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Achievement
{

    // TO DO:
    /*
     * Hacer que tenga una función de completarse
     * Hacer que tenga acceso a los datos del GameManager y del Jugador
     * Investigar si sus propiedades pueden ser asignadas desde el inspector
     * Investigar si las propiedades de sus hijos pueden ser asiganadas desde el inspector
     * Investigar si se pueden leer imágenes desde la carpeta de assets
     * 
     */
    private string title;
    private string description;
    private bool clear;

    public Image PendingImage;
    public Image ClearImage;

    private Image displayImage;

    public Achievement(string Title, Image pending, Image cleared)
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
