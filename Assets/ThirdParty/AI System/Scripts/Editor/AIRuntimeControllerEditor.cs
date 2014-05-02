using UnityEngine;
using UnityEditor;
using System.Collections;
using AISystem;

[CustomEditor(typeof(AIRuntimeController))]
public class AIRuntimeControllerEditor : Editor {
	private AIRuntimeController runtimeController;
	private void OnEnable(){
		runtimeController = (AIRuntimeController)target;
	}

	public override void OnInspectorGUI ()
	{
		GUI.changed = false;
		runtimeController.originalController = (AIController)EditorGUILayout.ObjectField ("Controller", runtimeController.originalController, typeof(AIController), false);
		runtimeController.level = EditorGUILayout.IntField ("Level", runtimeController.level);
		if (GUI.changed) {
			EditorUtility.SetDirty(target);
		}
	}
}
