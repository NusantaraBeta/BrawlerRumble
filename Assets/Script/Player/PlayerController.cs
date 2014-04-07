using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public int health;
	public float speed;

	MoveController _playerMovement;
	AnimController _playerAnim;

	public Transform HairPlace, weaponPlace;

	public static PlayerController Instance;
	private static PlayerController instance = null;
	public static PlayerController CekInstance 
	{
		get {return instance;}
	}

	void Awake()
	{
		Instance = this;

		if (instance != null && instance != this) 
		{
			Destroy(this.gameObject);
			return;
		}
		else 
		{
			instance = this;
		}
		
		DontDestroyOnLoad(this.gameObject);	
	}

	void Start()
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
	public float holdTimeNeeded;

	public void Attack()
	{
		ResetHold();
		_playerAnim.AttackCombo();
	}

	public void SpecialAttack()
	{
		ResetHold();
		_playerAnim.SpecialAttack();
	}

	void ResetHold()
	{
		isHold = false;
		holdTime = 0;
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
		if(holdTime>=holdTimeNeeded)
		{
			SpecialAttack();
		}
	}

	void OnCollisionEnter(Collision collisionObject)
	{

	}
}
