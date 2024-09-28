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
        transform.position += transform.right * Time.deltaTime * 25f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject Puddle = GameObject.Instantiate(puddle);
        Puddle.transform.position = this.gameObject.transform.position;
        Destroy(this.gameObject); //Actualizar por object pooling
    }
}
