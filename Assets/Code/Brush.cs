using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    private float cooldown = 5f;
    [SerializeField] private GameObject TheBullet;
    [SerializeField] Transform spawnpoint;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            GameObject BulletClone = GameObject.Instantiate(TheBullet);
            BulletClone.transform.position = spawnpoint.position;
            BulletClone.transform.rotation = spawnpoint.rotation;
            BulletClone.GetComponent<Rigidbody2D>().AddForce(spawnpoint.forward * 1000.0f);
        }
    }
}
