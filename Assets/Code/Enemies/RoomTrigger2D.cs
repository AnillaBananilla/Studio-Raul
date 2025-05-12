using UnityEngine;
using System.Collections.Generic;

public class RoomTrigger2D : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyInstance;    // El enemigo ya está en escena
        public Transform spawnPoint;        // Donde debe reaparecer (opcional si se mueve)
    }

    public string roomName;

    [Header("Enemigos de este cuarto")]
    public List<EnemySpawnData> enemies = new List<EnemySpawnData>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"[Trigger] Jugador ENTRÓ al cuarto: {roomName}");
            ActivateEnemies();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"[Trigger] Jugador SALIÓ del cuarto: {roomName}");
            DeactivateEnemies();
        }
    }

    private void ActivateEnemies()
    {
        foreach (var enemy in enemies)
        {
            if (enemy.enemyInstance != null && !enemy.enemyInstance.activeInHierarchy)
            {
                enemy.enemyInstance.transform.position = enemy.spawnPoint.position; // reposicionar si se requiere
                enemy.enemyInstance.SetActive(true);
            }
        }
    }

    private void DeactivateEnemies()
    {
        foreach (var enemy in enemies)
        {
            if (enemy.enemyInstance != null && enemy.enemyInstance.activeInHierarchy)
            {
                enemy.enemyInstance.SetActive(false);
            }
        }
    }
}
