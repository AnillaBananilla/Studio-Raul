using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public CharacterController2D controller;
 
    public float speed = 10.0f;
    public Animator animator;
    public GameManager gameManager;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (gameManager.isGameActive == true)
        {

        bool jump = false;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
        animator.SetBool("Move_Bool", Input.GetAxis("Horizontal") != 0);
        controller.Move(Input.GetAxis("Horizontal") * (speed/100), false, jump);
        }
    }
}
