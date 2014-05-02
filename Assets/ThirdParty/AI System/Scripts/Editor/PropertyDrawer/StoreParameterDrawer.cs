using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using AISystem;

[CustomPropertyDrawer(typeof(StoreParameterAttribute))]
public class StoreParameterDrawer : PropertyDrawer {

	StoreParameterAttribute storeAttribute { get { return ((StoreParameterAttribute)attribute); } }
	
	private bool initialized;
	private int selectedIndex;
	private AIController controller;
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		position.x += 4;
		position.width -= 4;
		if (!initialized) {
			AIEditorWindow[] windows=Resources.FindObjectsOfTypeAll<AIEditorWindow>();
			if(windows.Length >0){
				controller = windows[0].controller;
			}
			initialized=true;
		}
		
		if (controller != null) {
			
			string[] parameters=controller.GetParameterNames(storeAttribute.types);
			
			if(parameters.Length == 0 || !storeAttribute.required){
				System.Array.Resize (ref parameters, parameters.Length + 1);
				parameters[parameters.Length - 1] = storeAttribute._default;
				List<string> list= new List<string>(parameters);
				list.Swap(0,parameters.Length-1);
				parameters=list.ToArray();
			}
			
			for(int i=0;i< parameters.Length;i++){
				if(parameters[i] == property.stringValue){
					selectedIndex=i;
				}
			}
			
			if(parameters.Length>0){
				GUI.color=(storeAttribute.required && parameters.Length < 2 && property.stringValue == storeAttribute._default?Color.red:Color.white);
				selectedIndex=EditorGUI.Popup(position,label.text,selectedIndex,parameters);
				GUI.color=Color.white;
			}
			property.stringValue=parameters[selectedIndex];
		}
	}

}
