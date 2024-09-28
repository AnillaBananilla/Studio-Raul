using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    private IEnumerator CR_Countdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            Destroy(this.gameObject);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Activar el daño pasivo cuando entre un actor Enemigo
    }
}
