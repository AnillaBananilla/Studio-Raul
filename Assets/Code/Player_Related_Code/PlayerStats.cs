using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseMoveSpeed = 10f;
    public float baseRunSpeed = 40f;
    public int baseMaxHealth = 100;
    public int baseDamage = 10;
    public float baseDamageReduction = 0f;
    
    [Header("Current Stats")]
    public float moveSpeedModifier = 1f;
    public float runSpeedModifier = 1f;
    public int maxHealthModifier = 0;
    public int damageModifier = 0;
    public float damageReductionModifier = 0f;
    public float paintConsumptionChanceModifier = 1f;
    
    public float GetTotalMoveSpeed() => baseMoveSpeed * moveSpeedModifier;
    public float GetTotalRunSpeed() => baseRunSpeed * runSpeedModifier;
    public int GetTotalMaxHealth() => baseMaxHealth + maxHealthModifier;
    public int GetTotalDamage() => baseDamage + damageModifier;
    public float GetTotalDamageReduction() => baseDamageReduction + damageReductionModifier;
}