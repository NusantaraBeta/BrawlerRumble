using UnityEngine;
using System.Collections;

public class MoveController : MonoBehaviour {


	private Vector3 movement, horizontalVelocity;
	private Vector2 absJoyPos;

	private Transform cameraTransform, thisTransform;
	private CharacterController playerController;

	void Awake () {

		thisTransform = transform;
		playerController = this.GetComponent<CharacterController>();
	}

	void Update()
	{
		if(!cameraTransform)
		{
			cameraTransform = GameObject.FindWithTag("MainCamera").transform;
			SS_ShadowManager.Instance.RegisterShadow(GetComponent<SwiftShadow>());
		}

	}

	public void Move( float xAxis, float yAxis, float speed)
	{
		movement = cameraTransform.TransformDirection(new Vector3( xAxis, 0, yAxis ) );
		movement.y = 0;
		movement.Normalize(); 

		thisTransform.forward = movement;

		movement *= speed * 1;
		
		movement += Physics.gravity;
		movement *= Time.deltaTime;

		playerController.Move( movement*speed );
	}


}
