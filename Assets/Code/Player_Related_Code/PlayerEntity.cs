using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerEntity : Entity
{
    

    public int MaxHP = 100;
    public int MaxItems = 1;
    public int MaxPaint = 3;

    public string[] EquippedItems = new string[3]; //Por Revisar. No s� como podemos acceder a �tems desde ac�.
    public PlayerInventory Inventory;

    public char CurrentColor = 'n';
    public int Money;

    public int CPaint = 3;
    public int MPaint = 3;
    public int YPaint = 3;

    public int GetScene()
    {
        //Debug.Log(SceneManager.GetActiveScene().buildIndex);
        return SceneManager.GetActiveScene().buildIndex;
    }


    public override void Attack()
    {
        //Colocar aqu� las animaciones de Ataque. PlayerAttack.cs ya tiene la l�gica.
        Debug.Log("YOU ATTACK");
    }

    public override void Die()
    {
        //Mandar a la pantalla del inicio
        Debug.Log("YOU DIED");
    }

    public void Fall()
    {
        //El jugador recibe 5 puntos de da�o y respawnea en la �ltima entrada que tom� si sobrevive
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
