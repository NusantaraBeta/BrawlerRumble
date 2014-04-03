using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	public int health;
	public float speed;

	void OnTriggerEnter(Collider collisionObject)
	{
		Debug.Log("Kena Collider" + collisionObject);
	}

}
