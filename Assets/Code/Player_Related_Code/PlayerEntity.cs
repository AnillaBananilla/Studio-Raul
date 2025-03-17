using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerEntity : Entity
{
    

    public int MaxHP = 100;
    public int MaxItems = 1;
    public int MaxPaint = 3;

    public string[] EquippedItems = new string[3]; //Por Revisar
    public PlayerInventory Inventory;

    public char CurrentColor = 'n';
    public int Money;

    private int CPaint = 3;
    private int MPaint = 3;
    private int YPaint = 3;


    public override void Attack()
    {
        //Colocar aquí las animaciones de Ataque. PlayerAttack.cs ya tiene la lógica.
    }

    public override void Die()
    {
        //Mandar a la pantalla del inicio
    }

    public void Fall()
    {
        //El jugador recibe 5 puntos de daño y respawnea en la última entrada que tomó si sobrevive
    }

    public override void TakeDamage(int Damage, char color)
    {
        throw new System.NotImplementedException();
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
