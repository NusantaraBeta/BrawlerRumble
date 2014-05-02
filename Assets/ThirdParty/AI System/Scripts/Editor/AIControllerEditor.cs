using UnityEngine;
using System.Collections;
using UnityEditor;
using AISystem;

[CustomEditor(typeof(AIController))]
public class AIControllerEditor : Editor {
	
	private void OnEnable(){
		EditorApplication.projectWindowItemOnGUI += OnDoubleClickItem;
	}
	
	private void OnDisable(){
		EditorApplication.projectWindowItemOnGUI -= OnDoubleClickItem;
	}
	
	public override void OnInspectorGUI (){

		//base.OnInspectorGUI ();
	}
	
	public virtual void OnDoubleClickItem(string test,Rect r){
		if (Event.current.type == EventType.MouseDown && Event.current.clickCount == 2 && r.Contains (Event.current.mousePosition)) {
			AIEditorWindow.Init((AIController)target);
		}
	}
}
