using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementNew : MonoBehaviour
{
    public Rigidbody2D rb;

    [SerializeField] public InputHandler input;

    bool isFacingRight = true;

    public Animator animator;
    float currSpeed;
    public bool canMove;

    [Header("Walking")]
    public float walkSpeed = 10f;
    float horizontalMovement;

    [Header("Running")]
    public float runSpeed = 40f;
    public bool isRunning = false;

    [Header("Jump")]
    public float jumpForce = 10f;
    public int maxJumps = 2;
    public int jumpsLeft;

    [Header("Ground Check")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;

    [Header("Dropping")]
    public bool isGrounded;
    public bool isOnPlatform;
    public bool isDropping = false;

    [Header("Crouch")]
    public CapsuleCollider2D upColl;
    public BoxCollider2D crouchColl;

    [Header("Roof Check")]
    public Transform roofCheckPos;
    public Vector2 roofCheckSize = new Vector2(0.5f, 0.5f);
    bool canGetUp = true;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMulti = 2f;

    // Agrega esto al inicio de la clase
    [SerializeField] private PlayerStats playerStats;

// Modifica estas líneas en Update():


    void Update()
    {
        if (input.moveable)
        {
            if (!canMove)
            {
                rb.velocity = Vector2.zero;
                return;
            }

            currSpeed = isRunning ? playerStats.currentRunSpeed : playerStats.currentMoveSpeed;

            GroundCheck();
            //RoofCheck();
            Gravity();
            Flip();

            animator.SetFloat("yVelocity", rb.velocity.y);
            animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        }
           
    }

    void FixedUpdate()
    {
        if (input.moveable)
        {
            rb.velocity = new Vector2(horizontalMovement * currSpeed, rb.velocity.y);
        }
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(jumpsLeft > 0 && input.moveable)
        {
            if (context.performed)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpsLeft--;
                //animator.SetTrigger("Jump");
            }
            else if (context.canceled)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                jumpsLeft--;
                //animator.SetTrigger("Jump");
            }
        }
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            jumpsLeft = maxJumps;
            isGrounded = true;
        }
        else{
            isGrounded = false;
        }
    }
    /*
    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            crouchColl.enabled = true;
            upColl.enabled = false;
            animator.SetBool("isCrouching", true);
        }
        else if(context.canceled && canGetUp && !isDropping)
        {
            crouchColl.enabled = false;
            upColl.enabled = true;
            animator.SetBool("isCrouching", false);
        }
    }
    
    private void RoofCheck()
    {
        if (Physics2D.OverlapBox(roofCheckPos.position, roofCheckSize, 0, groundLayer))
        {
            canGetUp = false;

        }
        else 
        {
            canGetUp = true;
            if (!Keyboard.current.ctrlKey.isPressed && !isDropping)
            {
                crouchColl.enabled = false;
                upColl.enabled = true;
                animator.SetBool("isCrouching", false);
            }
        }
    } */

    public void Drop(InputAction.CallbackContext context){
        if(context.performed && isGrounded && isOnPlatform && upColl.enabled){
            StartCoroutine(DisablePlayerCollider(0.5f));
        }
    }

    private IEnumerator DisablePlayerCollider(float disableTime){
        isDropping = true; 
        upColl.enabled = false;
        yield return new WaitForSeconds(disableTime);
        upColl.enabled = true;
        isDropping = false; 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Platform")){
            isOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Platform")){
            isOnPlatform = false;
        }
    }

    private void Gravity()
    {
        if(rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMulti;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void Flip()
    {
        if(isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }    
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.DrawWireCube(roofCheckPos.position, roofCheckSize);
    }
}
