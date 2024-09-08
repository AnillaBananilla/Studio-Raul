using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droplets : MonoBehaviour
{
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
        
    }
}
