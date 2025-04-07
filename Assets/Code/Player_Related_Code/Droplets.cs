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
            yield return new WaitForSeconds(2);
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
            if (collision.gameObject.CompareTag("Ground"))
            {
                GameObject Puddle = GameObject.Instantiate(puddle);
                Puddle.transform.position = this.gameObject.transform.position;
                PoolManager.Instance.ReturnObjectToPool(this.gameObject);
            }
            else
            {
                if (collision.gameObject.CompareTag("Enemy"))
                {
                    collision.gameObject.GetComponent<Entity>().TakeDamage(35);
                    //collision.gameObject.GetComponent<AttackReceiver>().ReceiveDamage(35);
                }
                PoolManager.Instance.ReturnObjectToPool(this.gameObject);

            }
                

        }

    }
            
}
