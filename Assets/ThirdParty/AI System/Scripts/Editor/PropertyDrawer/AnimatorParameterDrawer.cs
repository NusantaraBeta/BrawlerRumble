using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using AISystem;

[CustomPropertyDrawer(typeof(AnimatorParameterAttribute))]
public class AnimatorParameterDrawer : PropertyDrawer {
	private string[] triggerNames;
	private string[] intNames;
	private string[] boolNames;
	private string[] floatNames;
	protected string[] stateNames;
	private bool executed;
	private AIController controller;
	private AnimatorParameterAttribute paramterAttribute{
		get{
			return (AnimatorParameterAttribute)attribute;
		}
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		
		position.x += 4;
		position.width -= 6;
		if (!executed) {
			AIEditorWindow[] windows=Resources.FindObjectsOfTypeAll<AIEditorWindow>();
			if(windows.Length >0){
				controller = windows[0].controller;
			}
			if(controller != null && controller.runtimeAnimatorController != null){
				FillStateNames (controller.runtimeAnimatorController);
				FillParameterArray (controller.runtimeAnimatorController, AnimatorControllerParameterType.Float);
				FillParameterArray (controller.runtimeAnimatorController, AnimatorControllerParameterType.Bool);
				FillParameterArray (controller.runtimeAnimatorController, AnimatorControllerParameterType.Int);
				FillParameterArray (controller.runtimeAnimatorController, AnimatorControllerParameterType.Trigger);
			}
			executed = true;
		}

		if (controller!= null && controller.runtimeAnimatorController) {
			switch (paramterAttribute.type) {
			case AnimatorParameter.Bool:
				property.stringValue = UnityEditorTools.StringPopup (position, label.text, property.stringValue, boolNames);
				break;
			case AnimatorParameter.Float:
				property.stringValue = UnityEditorTools.StringPopup (position, label.text, property.stringValue, floatNames);
				break;
			case AnimatorParameter.Int:
				property.stringValue = UnityEditorTools.StringPopup (position, label.text, property.stringValue, intNames);
				break;
			case AnimatorParameter.Trigger:
				property.stringValue = UnityEditorTools.StringPopup (position, label.text, property.stringValue, triggerNames);
				break;
			case AnimatorParameter.State:
				property.stringValue = UnityEditorTools.StringPopup (position, label.text, property.stringValue, stateNames);
				break;
			}
		} else {
			EditorGUI.PropertyField(position,property,label);
		}
	}

	public void FillStateNames(RuntimeAnimatorController animator){
		List<string> names = new List<string> ();
		int layerCount =(animator as AnimatorController).layerCount;
		for (int layer = 0; layer < layerCount; layer++) {
			StateMachine stateMachine = (animator as AnimatorController).GetLayer(layer).stateMachine;
			int stateCount=stateMachine.stateCount;
			for (int state=0;state<stateCount;state++) {
				names.Add(stateMachine.GetState(state).uniqueName);
			}
		}
		stateNames = names.ToArray ();
	}
	
	
	public void FillParameterArray(RuntimeAnimatorController animator,AnimatorControllerParameterType type){
		AnimatorController animatorController = animator as AnimatorController;
		List<string> parameterNames = new List<string> ();
		if (animatorController.parameterCount > 0) {
			for (int i=0; i< animatorController.parameterCount; i++) {
				if (animatorController.GetParameter (i).type == type) {
					parameterNames.Add (animatorController.GetParameter (i).name);
				}
			}
			switch(type){
			case AnimatorControllerParameterType.Bool:
				boolNames = parameterNames.ToArray ();
				break;
			case AnimatorControllerParameterType.Float:
				floatNames = parameterNames.ToArray ();
				break;
			case AnimatorControllerParameterType.Int:
				intNames = parameterNames.ToArray ();
				break;
			case AnimatorControllerParameterType.Trigger:
				triggerNames = parameterNames.ToArray ();
				break;
				
			}
		}
	}
}
