using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public MetroidCharacterController2D controller;
	public Animator animator;

	public float walkSpeed = 10f; //Mine
	public float runSpeed = 40f;

	private bool isSprinting = false;

	float horizontalMove = 0f;
	bool jump = false;
	bool dash = false;

	//bool dashAxis = false;
	
	// Update is called once per frame
	void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (Input.GetKeyDown(KeyCode.Space))
		{
			jump = true;
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			dash = true;
		}

        // Velocidad de carrera
        float currentSpeed = isSprinting ? runSpeed : walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
			runSpeed = 75f;
        }
        else
        {
            isSprinting = false;
			runSpeed = 40f;
        }
        /* Animación del movimiento
        if (Mathf.Abs(inputH) > 0)
        {
            animator.SetFloat("Speed", currentSpeed);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }*/


        /*if (Input.GetAxisRaw("Dash") == 1 || Input.GetAxisRaw("Dash") == -1) //RT in Unity 2017 = -1, RT in Unity 2019 = 1
		{
			if (dashAxis == false)
			{
				dashAxis = true;
				dash = true;
			}
		}
		else
		{
			dashAxis = false;
		}
		*/

    }

	public void OnFall()
	{
		animator.SetBool("IsJumping", true);
	}

	public void OnLanding()
	{
		animator.SetBool("IsJumping", false);
	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
		jump = false;
		dash = false;
	}
}
