using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healt : MonoBehaviour
{
    public int currentHealt;
    public int maxHealt;

    public GameObject coinPrefab;
    public float coinOffsetY = -5f;
    [Range(0, 100)] public float coinSpawnChance = 50f; // Probabilidad en porcentaje

    void Start()
    {
        currentHealt = maxHealt;
    }

    void Update()
    {
        
    }

    public void Damage(int damage)
    {
        currentHealt -= damage;

        if (currentHealt <= 0)
        {
            Die();
        }
    }

    public void Heal(int heal)
    {
        currentHealt += heal;
        if (currentHealt > maxHealt)
        {
            currentHealt = maxHealt;
        }
    }

    public void Die()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            EventManager.m_Instance.InvokeEvent<DieEvent>(new DieEvent());
            currentHealt = maxHealt;
        }
        else
        {
            Destroy(gameObject);

            // Determinar si se debe crear la moneda con base en la probabilidad
            if (UnityEngine.Random.Range(0f, 100f) <= coinSpawnChance)
            {
                Vector3 coinPosition = new Vector3(transform.position.x, transform.position.y + coinOffsetY, transform.position.z);
                Instantiate(coinPrefab, coinPosition, Quaternion.identity);
            }
        }
    }
}