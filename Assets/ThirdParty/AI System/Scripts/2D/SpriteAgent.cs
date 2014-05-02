using UnityEngine;
using System.Collections;

public class SpriteAgent : MonoBehaviour {
	public float radius;
	public float height;
	public Vector2 center;
	[HideInInspector]
	public bool isOnJumpLink;
	[HideInInspector]
	public JumpLink currentJumpLink;

	public PhysicsMaterial2D physicsMaterial;
	public Vector2 position{
		get{
			return new Vector2(transform.position.x,transform.position.y-height-center.y);
		}
		set{
			transform.position = new Vector3(value.x,transform.position.y,0);
		}
	}

	private void Awake(){
		float relY = (1 / (transform.localScale.y / center.y));
		float relHeight=(1/(transform.localScale.y/height));

		BoxCollider2D box = gameObject.AddComponent<BoxCollider2D> ();
		box.center = new Vector2(center.x,relY);
		box.size = new Vector2 (radius, relHeight);

		CircleCollider2D up = gameObject.AddComponent<CircleCollider2D> ();
		up.radius = radius*0.5f;
		up.center = box.center + Vector2.up*relHeight*0.5f;
		up.sharedMaterial = physicsMaterial;

		CircleCollider2D groundCollider = gameObject.AddComponent<CircleCollider2D> ();
		groundCollider.radius = radius*0.5f;
		groundCollider.center = box.center - Vector2.up*relHeight*0.5f;
	}
}
