using UnityEngine;

public class RoomTrigger2D : MonoBehaviour
{
    public string roomName;
    public EnemyManager enemyManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemyManager.SetRoomState(roomName, true);
            Debug.Log($"[Trigger] Jugador ENTRÓ al cuarto: {roomName}");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemyManager.SetRoomState(roomName, false);
            Debug.Log($"[Trigger] Jugador SALIÓ del cuarto: {roomName}");
        }
    }
}
