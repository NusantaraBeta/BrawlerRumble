using UnityEngine;
using System.Collections;

public class MoveController : MonoBehaviour {


	private Vector3 movement, horizontalVelocity;
	private Vector2 absJoyPos;

	private Transform cameraTransform, thisTransform;
	private Rigidbody playerController;

	void Awake () {

		thisTransform = transform;
		playerController = this.GetComponent<Rigidbody>();
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

		//Kalo pakeCharacterController
		/*movement += Physics.gravity;
		movement *= Time.deltaTime;
		playerController.Move(movement*speed );
		 */

		//Kalo pake Rigidbody
		movement *= Time.deltaTime;
		playerController.MovePosition( playerController.position + movement*speed );
	}


}
