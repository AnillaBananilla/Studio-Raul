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

    public string[] EquippedItems = new string[3]; //Por Revisar. No sé como podemos acceder a ítems desde acá.
    public PlayerInventory Inventory;

    public char CurrentColor = 'n';
    public int Money;

    public int CPaint = 3;
    public int MPaint = 3;
    public int YPaint = 3;


    public override void Attack()
    {
        //Colocar aquí las animaciones de Ataque. PlayerAttack.cs ya tiene la lógica.
        Debug.Log("YOU ATTACK");
    }

    public override void Die()
    {
        //Mandar a la pantalla del inicio
        Debug.Log("YOU DIED");
    }

    public void Fall()
    {
        //El jugador recibe 5 puntos de daño y respawnea en la última entrada que tomó si sobrevive
    }

    public override void TakeDamage(int Damage, char color)
    {
        // Checar si el jugador tiene equipado algún item de reducción de daño, deducir la protección.
        ///if (Equipped Items
        HP -= Damage;
        if (HP < 0)
        {
            HP = 0;
            Die();
        }
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
