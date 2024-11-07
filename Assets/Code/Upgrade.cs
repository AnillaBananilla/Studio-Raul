using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Touched me!");
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.RangeUpgrade();
            Destroy(this.gameObject);
        }
    }

}
