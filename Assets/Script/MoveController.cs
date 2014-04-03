using UnityEngine;
using System.Collections;

public class MoveController : MonoBehaviour {


	private Vector3 movement, horizontalVelocity;
	private Vector2 absJoyPos;

	private Transform cameraTransform, thisTransform;
	private CharacterController playerController;


	void Awake () {
		cameraTransform = GameObject.FindWithTag("MainCamera").transform;

		thisTransform = transform;
		playerController = this.GetComponent<CharacterController>();
	}

	void FaceMovementDirection()
	{	
		horizontalVelocity = playerController.velocity;
		horizontalVelocity.y = 0;
		
		if ( horizontalVelocity.magnitude > 0.1 )
			thisTransform.forward = horizontalVelocity.normalized;
	}

	public void Move( float xAxis, float yAxis, float speed)
	{
		movement = cameraTransform.TransformDirection(new Vector3( xAxis, 0, yAxis ) );
		movement.y = 0;
		movement.Normalize(); 
		
		movement *= speed;
		
		movement += Physics.gravity;
		movement *= Time.deltaTime;
		
		playerController.Move( movement*speed );
		
		FaceMovementDirection();
	}


}
