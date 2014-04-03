using UnityEngine;

public class SS_MoveRotate : MonoBehaviour {
    public Vector3 RotationSpeed = new Vector3(0f, 20f, 0f);
    public float MoveForwardSpeed = 0f;
    public float MoveRightSpeed = 0f;

	// Update is called once per frame
	void Update () {
	    transform.localRotation *= Quaternion.Euler(RotationSpeed * Time.deltaTime);
	    transform.localPosition += transform.forward * MoveForwardSpeed;
	    transform.localPosition += transform.right * MoveRightSpeed;
	}
}
