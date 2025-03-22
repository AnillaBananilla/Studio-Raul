using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droplets : MonoBehaviour
{
    public GameObject puddle;

    private IEnumerator CR_Countdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            //Destroy(this.gameObject);
            PoolManager.Instance.ReturnObjectToPool(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CR_Countdown());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            if (!collision.gameObject.CompareTag("Obstacle") && (!collision.gameObject.CompareTag("PlayerProjectile")))
            {
                GameObject Puddle = GameObject.Instantiate(puddle);
                Puddle.transform.position = this.gameObject.transform.position;
                PoolManager.Instance.ReturnObjectToPool(this.gameObject);
            }
            else
            {
                PoolManager.Instance.ReturnObjectToPool(this.gameObject);

            }
                

        }

    }
            
}
