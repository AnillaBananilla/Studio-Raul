using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public MetroidCharacterController2D controller;
    public Animator animator;
    public float runSpeed = 40f;

    private Vector2 moveInput;
    private bool jump = false;
    private bool dash = false;
	private bool menuOpen = false;

	public bool pressMenu = false;
	public bool pressEquip = false;
	public bool isNavigatingLeft;
	public bool isNavigatingRight;
	public bool isSelecting;
    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if(!menuOpen){
		moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        jump = playerInput.actions["Jump"].WasPerformedThisFrame();
        dash = playerInput.actions["Dash"].WasPerformedThisFrame();
		}
		
		pressMenu = playerInput.actions["OpenMenu"].WasPerformedThisFrame();
   		pressEquip = playerInput.actions["Equip"].WasPerformedThisFrame();

		isNavigatingLeft = playerInput.actions["NavigateLeft"].WasPerformedThisFrame();
    	isNavigatingRight = playerInput.actions["NavigateRight"].WasPerformedThisFrame();
		isSelecting = playerInput.actions["SelectItem"].WasPerformedThisFrame();
		

        animator.SetFloat("Speed", Mathf.Abs(moveInput.x * runSpeed));
    }

    void FixedUpdate()
    {
        controller.Move(moveInput.x * runSpeed * Time.fixedDeltaTime, jump, dash);
		jump = false;
		dash = false;
    }


	public void OnFall()
	{
		animator.SetBool("IsJumping", true);
	}

	public void OnLanding()
	{
		animator.SetBool("IsJumping", false);
	}
}
