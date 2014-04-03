using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public int health;
	public float speed;

	MoveController _playerMovement;
	AnimController _playerAnim;

	public Transform HairPlace, weaponPlace;

	void Awake()
	{
		_playerAnim = GetComponent<AnimController>();
		_playerMovement = GetComponent<MoveController>();
	}

	void OnEnable(){
		EasyJoystick.On_JoystickMove += On_JoystickMove;	
		EasyJoystick.On_JoystickMoveEnd += On_JoystickMoveEnd;
	}
	
	void OnDisable(){
		EasyJoystick.On_JoystickMove -= On_JoystickMove;	
		EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
	}
	
	void OnDestroy(){
		EasyJoystick.On_JoystickMove -= On_JoystickMove;	
		EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
	}

	void On_JoystickMove( MovingJoystick move)
	{
		if(!_playerAnim.getAnimBoolVar("isAttack"))
		{
			_playerMovement.Move(move.joystickAxis.x, move.joystickAxis.y, speed);
			_playerAnim.SetAnimVar("isRun", true);
		}
	}
	
	void On_JoystickMoveEnd (MovingJoystick move)
	{
		_playerAnim.SetAnimVar("isRun", false);
	}

	private bool isHold = false;
	private float holdTime=0f;

	public void Attack()
	{
		isHold=false;
		holdTime=0;
		_playerAnim.AttackCombo();
	}

	public void SpecialAttack()
	{
		_playerAnim.SpecialAttack();
	}

	public void HoldButton()
	{
		isHold = true;
	}

	void Update()
	{
		if(isHold)
		{
			holdTime += Time.deltaTime;
		}
	}

	void LateUpdate()
	{
		if(holdTime>0.5f)
		{
			SpecialAttack();
			isHold = false;
			holdTime = 0;
		}
	}
}
