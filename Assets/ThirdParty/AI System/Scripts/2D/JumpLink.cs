using UnityEngine;
using System.Collections;

public class JumpLink : MonoBehaviour {
	public float agentRadius;
	public float traverseTime=1.0f;
	public Transform start;
	public Transform end;
	public bool biDirectional;
	
	private void Start(){
		CircleCollider2D startCollider = start.gameObject.AddComponent<CircleCollider2D> ();
		startCollider.radius = agentRadius*0.5f;
		startCollider.isTrigger = true;
		
		if (biDirectional) {		
			JumpLink link=end.gameObject.AddComponent<JumpLink>();
			link.traverseTime=traverseTime;
			link.start=end;
			link.end=start;
			link.agentRadius=agentRadius;
		}
	}
	
	private void OnTriggerEnter2D(Collider2D other){
		SpriteAgent agent = other.GetComponent<SpriteAgent> ();
		if (agent != null) {
			Vector2 dir=new Vector2(end.position.x,end.position.y) - agent.position;
			if(agent.transform.localScale.x > 0 && dir.x > 0 || agent.transform.localScale.x < 0 && dir.x < 0){
				agent.currentJumpLink = this;
				agent.isOnJumpLink = true;
			}
		}
	}
}
