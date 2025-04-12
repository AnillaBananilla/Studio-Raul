using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Healt : MonoBehaviour
{
    public int currentHealt;
    public int maxHealt;

    public GameObject coinPrefab;
    public float coinOffsetY = -5f;
    [Range(0, 100)] public float coinSpawnChance = 50f;
    
    public GameObject damageTextPrefab; // Prefab del texto de da�o
    public Vector3 damageTextOffset = new Vector3(0, 3f, 0); // Offset para la posici�n del texto

    void Start()
    {

        currentHealt = maxHealt;

    }

    void Update()
    {

    }

    public void Damage(int damage)
    {
        // TODO:
        /*
         * Que se apliquen los cambios en daño de Player Stats.
         * Meter un switch que cheque el tag del enemigo, y modifique el daño que recibe.
         * 
         */
        currentHealt -= damage;
        ShowDamageText(-damage); 

        if (currentHealt <= 0)
        {
            Die();
        }
    }

    public void Heal(int heal)
    {
        /* TODO
        * Que se aplique el boost de curas
        */
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

            if (UnityEngine.Random.Range(0f, 100f) <= coinSpawnChance)
            {
                Vector3 coinPosition = new Vector3(transform.position.x, transform.position.y + coinOffsetY, transform.position.z);
                Instantiate(coinPrefab, coinPosition, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    private void ShowDamageText(int damage)
    {
        if (damageTextPrefab != null)
        {
            GameObject damageTextInstance = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);

            // Buscar el TextMeshPro dentro del prefab
            TextMeshProUGUI textMesh = damageTextInstance.GetComponentInChildren<TextMeshProUGUI>();
            if (textMesh != null)
            {
                textMesh.text = damage.ToString();
            }
         
            Destroy(damageTextInstance, 1f); // Destruir el texto despu�s de 1 segundo
        }
    }
}