using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	public int health;
	public float speed;


	void OnCollisionEnter(Collision collisionObject)
	{
		Debug.Log("Kena Collision" + collisionObject);
	}
}
