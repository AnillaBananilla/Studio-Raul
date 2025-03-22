using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerSkills;

public class InputHandler : MonoBehaviour
{
	[Header("Acciones referentes al inventario")]
	public bool pressMenu = false;
	public bool pressEquip = false;
	public bool isNavigatingLeft;
	public bool isNavigatingRight;
	public bool isSelecting;

	[Header("Instancia del playerInput del player")]
    private PlayerInput playerInput;

	[Header("Sobre ataque")]
	public bool attack;
    public bool attackPaint;
    public UnlockPincel unlockPincel;
    public PlayerSkills playerSkills;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

    }

    void Update()
    {
        if (unlockPincel.canAttack)
        {
            attack = playerInput.actions["Attack"].WasPerformedThisFrame();
            attackPaint = playerInput.actions["AttackPaint"].WasPerformedThisFrame();
        }
        


		//acciones relacionadas al inventario
		pressMenu = playerInput.actions["OpenMenu"].WasPerformedThisFrame();
   		pressEquip = playerInput.actions["Equip"].WasPerformedThisFrame();

		isNavigatingLeft = playerInput.actions["NavigateLeft"].WasPerformedThisFrame();
    	isNavigatingRight = playerInput.actions["NavigateRight"].WasPerformedThisFrame();
		isSelecting = playerInput.actions["SelectItem"].WasPerformedThisFrame();
    }
}
