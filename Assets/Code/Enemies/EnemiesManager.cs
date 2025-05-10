using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyInstance;   // El enemigo ya está en escena
        public Transform spawnPoint;       // Transform que indica dónde respawnea
    }

    [System.Serializable]
    public class RoomData
    {
        public string roomName;
        public bool playerInside = false;
        public List<EnemySpawnData> enemies;
    }

    public List<RoomData> rooms;

    void Update()
    {
        foreach (var room in rooms)
        {
           if (room.playerInside)
{
    foreach (var enemy in room.enemies)
    {
        if (enemy.enemyInstance != null && !enemy.enemyInstance.activeInHierarchy)
        {
            //revivan
            enemy.enemyInstance.SetActive(true);
        }
    }
}
            else
            {
                foreach (var enemy in room.enemies)
                {
                    if (enemy.enemyInstance != null && enemy.enemyInstance.activeInHierarchy)
                    {
                        enemy.enemyInstance.SetActive(false);
                    }
                }
            }
        }
    }

    // Llamado por RoomTrigger2D
    public void SetRoomState(string roomName, bool isInside)
    {
        foreach (var room in rooms)
        {
            if (room.roomName == roomName)
            {
                room.playerInside = isInside;
                break;
            }
        }
    }
}
