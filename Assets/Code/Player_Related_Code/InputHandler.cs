using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerSkills;

public class InputHandler : MonoBehaviour
{
    public bool moveable = true;

	[Header("Acciones referentes al inventario")]
	public bool pressMenu = false;
	public bool pressEquip = false;

	public bool useItem = false;
	public bool isNavigatingLeft;
	public bool isNavigatingRight;
	public bool isSelecting;

    [Header("Interacción con el entorno")]
    public bool interacting = false;

	[Header("Instancia del playerInput del player")]
    private PlayerInput playerInput;

	[Header("Sobre ataque")]
	public bool attack;
    public bool attackPaint;
    public bool changeColor;
    public UnlockPincel unlockPincel;
    public PlayerSkills playerSkills;

    public bool pressPause;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

    }

    void Update()
    {
        if (moveable)
        {
            if (GameManager.instance.BrushSkill())
            {
                attack = playerInput.actions["Attack"].WasPerformedThisFrame();
                attackPaint = playerInput.actions["AttackPaint"].WasPerformedThisFrame();
                changeColor = playerInput.actions["ChangeColor"].WasPerformedThisFrame();
            }



            //acciones relacionadas al inventario
            pressMenu = playerInput.actions["OpenMenu"].WasPerformedThisFrame();
            pressEquip = playerInput.actions["Equip"].WasPerformedThisFrame();
            useItem = playerInput.actions["UseItem"].WasPerformedThisFrame();

            isNavigatingLeft = playerInput.actions["NavigateLeft"].WasPerformedThisFrame();
            isNavigatingRight = playerInput.actions["NavigateRight"].WasPerformedThisFrame();
            isSelecting = playerInput.actions["SelectItem"].WasPerformedThisFrame();
            pressPause = playerInput.actions["Pause"].WasPerformedThisFrame();
            interacting = playerInput.actions["Interact"].WasPerformedThisFrame();
        }
        /*
         * if (GameManager.instance.BrushSkill())
        {
            attack = playerInput.actions["Attack"].WasPerformedThisFrame();
            attackPaint = playerInput.actions["AttackPaint"].WasPerformedThisFrame();
            changeColor = playerInput.actions["ChangeColor"].WasPerformedThisFrame();
        }
        


		//acciones relacionadas al inventario
		pressMenu = playerInput.actions["OpenMenu"].WasPerformedThisFrame();
   		pressEquip = playerInput.actions["Equip"].WasPerformedThisFrame();
		useItem = playerInput.actions["UseItem"].WasPerformedThisFrame();

		isNavigatingLeft = playerInput.actions["NavigateLeft"].WasPerformedThisFrame();
    	isNavigatingRight = playerInput.actions["NavigateRight"].WasPerformedThisFrame();
		isSelecting = playerInput.actions["SelectItem"].WasPerformedThisFrame();
        pressPause = playerInput.actions["Pause"].WasPerformedThisFrame();
        interacting = playerInput.actions["Interact"].WasPerformedThisFrame();
        */
    }
}
