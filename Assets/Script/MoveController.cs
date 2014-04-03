using UnityEngine;
using System.Collections;

public class MoveController : MonoBehaviour {


	private Vector3 movement, horizontalVelocity;
	private Vector2 absJoyPos;

	private Transform cameraTransform, thisTransform;
	private Rigidbody playerRigidbody;

	void Awake () {
		cameraTransform = GameObject.FindWithTag("MainCamera").transform;

		thisTransform = transform;
		playerRigidbody = this.GetComponent<Rigidbody>();
	}

	public void Move( float xAxis, float yAxis, float speed)
	{
		movement = cameraTransform.TransformDirection(new Vector3( xAxis, 0, yAxis ) );
		movement.y = 0;
		movement.Normalize(); 

		playerRigidbody.MovePosition(rigidbody.position + movement*speed );

		thisTransform.forward = movement;

	}


}
