using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Inventory Scanner")]
    public PlayerInventory playerInventory;
    
    [Header("Current Stats")]
    public float currentMoveSpeed = 15f;
    public float currentRunSpeed = 25f;
    public int currentMaxHealth = 100;
    public int currentDamage = 10;
    public float currentDamageReduction = 0f;
    
    [Header("Modified Stats")]
    private float moveSpeedModifier = 20f;
    private float runSpeedModifier = 35f;
    private int maxHealthModifier = 130;
    private int damageModifier = 20;
    private float damageReductionModifier = 20f;
    private float paintConsumptionChanceModifier = 1f;

    public void Update()
    {
        if(playerInventory.items[1].isEquipped){
            //Item 1 es CollarSol... AUMENTA VIDA +30pts
            currentMaxHealth = maxHealthModifier;
        }else{
            currentMaxHealth = 100;
        }
        if(playerInventory.items[2].isEquipped){
             //Item 2 es Frasco Pintura Negra... AUMENTA VELOCIDAD a 35
            currentMoveSpeed = moveSpeedModifier;
            currentRunSpeed = runSpeedModifier;
        }else{
            currentMoveSpeed = 15f;
            currentRunSpeed = 25;

        }
        if(playerInventory.items[3].isEquipped){
             //Item 3 es Pez Estrella Boca... AUMENTA DAÑO GENERADO 20 puntos
            currentDamage = damageModifier;
        }else{
            currentDamage = 0;
        }
        if(playerInventory.items[4].isEquipped){
            //Item 4 es Sol Gato... AUMENTA REDUCE DAÑO RECIBIDO EN 20 puntos menos
            currentDamageReduction = damageReductionModifier;
        }else{
            currentDamageReduction = 0;
        }
    }
}