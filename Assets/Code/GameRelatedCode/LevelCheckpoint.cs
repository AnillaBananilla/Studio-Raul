using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCheckpoint : MonoBehaviour
{
    [SerializeField] private int ID;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LevelNavigator.Instance.UpdateMap(ID);

            EventManager.m_Instance.InvokeEvent<MapUpdateEvent>(new MapUpdateEvent());
            Debug.Log("MAP UPDATE");
        }
    }
}
