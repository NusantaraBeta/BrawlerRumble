using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using AISystem;
using AISystem.States;
using System.Linq;

[CustomPropertyDrawer(typeof(AttributePopupAttribute))]
public class AttributePopupDrawer : PropertyDrawer {
	private AIController controller;
	private bool initialized;
	private string selected;
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		if (!initialized) {
			AIEditorWindow[] windows=Resources.FindObjectsOfTypeAll<AIEditorWindow>();
			if(windows.Length >0){
				controller = windows[0].controller;
			}
			initialized=true;
		}


		if (controller != null) {
			List<BaseAttribute> attributes = new List<BaseAttribute> ();
			for(int i=0;i< controller.states.Count;i++){
				if(controller.states[i] is OnAttributeChanged){
					attributes.Add((controller.states[i] as OnAttributeChanged).attribute);
				}
			}

			List<string> attributeNames= new List<string>();
			attributeNames.AddRange(attributes.Select(x=> x.name).ToArray());
			int count=attributeNames.Count;
			if(count== 0){
				attributeNames.Add("None");
			}
			//EditorGUI.BeginChangeCheck();
			GUI.color=count==0?Color.red:Color.white;
			position.x+=4;
			position.width-=4;
			selected=UnityEditorTools.StringPopup(position,label.text,selected,attributeNames.ToArray());
			GUI.color=Color.white;
			//if (EditorGUI.EndChangeCheck()){  
				property.stringValue = selected;
			//}

		}
	}
}
