using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public float horizontalInput;
    public float speed = 50.0f;
    public float fuerzaSalto = 10f;
    public Rigidbody2D rb;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        }

    }

}
