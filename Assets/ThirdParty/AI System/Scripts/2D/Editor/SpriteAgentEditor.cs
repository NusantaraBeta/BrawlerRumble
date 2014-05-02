using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SpriteAgent))]
public class SpriteAgentEditor : Editor {
	private SpriteAgent agent;

	private void OnEnable(){
		agent = (SpriteAgent)target;
	}

	private void OnSceneGUI(){
		Vector3 center =agent.transform.position+ new Vector3(agent.center.x,agent.center.y);
		Handles.DrawSolidDisc (center, Vector3.back, 0.01f);
		Handles.color = Color.blue;
		Vector3[] verts = new Vector3[]{
			new Vector3(center.x - agent.radius*agent.transform.localScale.x*0.5f,center.y-agent.height*0.5f,0), 
			new Vector3(center.x - agent.radius*agent.transform.localScale.x*0.5f,center.y+agent.height*0.5f,0), 
			new Vector3(center.x + agent.radius*agent.transform.localScale.x*0.5f,center.y+agent.height*0.5f,0), 
			new Vector3(center.x + agent.radius*agent.transform.localScale.x*0.5f,center.y-agent.height*0.5f,0)};

		Handles.DrawSolidRectangleWithOutline(verts, Color.clear,Color.blue);

		Handles.DrawWireArc(center- Vector3.up*agent.height*0.5f, 
		                    Vector3.back, 
		                    agent.transform.right, 
		                    180, 
		                    agent.radius*agent.transform.localScale.x*0.5f);

		Handles.DrawWireArc(center+ Vector3.up*agent.height*0.5f, 
		                    Vector3.back, 
		                    -agent.transform.right, 
		                    180, 
		                    agent.radius*agent.transform.localScale.x*0.5f);
		Handles.color = Color.white;
	}
}
