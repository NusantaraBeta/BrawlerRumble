using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using AISystem;

[CustomPropertyDrawer(typeof(NamedParameter),true)]
public class ParameterDrawer : PropertyDrawer {
	
	private bool initialized;
	private int selectedIndex;
	private AIController controller;
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		position.x += 4;
		position.width -= 6;
		if (!initialized) {
			AIEditorWindow[] windows=Resources.FindObjectsOfTypeAll<AIEditorWindow>();
			if(windows.Length >0){
				controller = windows[0].controller;
			}
			initialized=true;
		}
		
		if (controller != null) {
			NamedParameter param=(NamedParameter)property.objectReferenceValue;
			if(param == null){
				property.objectReferenceValue=param=CreateUserParameter();
			}
			position.width-=15;
			Rect r= new Rect(position);
			r.x+=position.width;
			r.width=20;

			param.userVariable=GUI.Toggle(r,param.userVariable,GUIContent.none);

			if(param != null && param.userVariable){
				param.Name=string.Empty;
				SerializedObject paramObject= new SerializedObject(param);
				paramObject.Update();
				EditorGUI.PropertyField(position,paramObject.FindProperty("value"),new GUIContent(label.text));
				paramObject.ApplyModifiedProperties();
			}else{
				string[] parameters=null;
				if(fieldInfo.FieldType == typeof(Vector3Parameter)){
					parameters=controller.GetParameterNames(fieldInfo.FieldType,typeof(GameObjectParameter));
				}else{
					parameters=controller.GetParameterNames(fieldInfo.FieldType);
				}
				if(parameters.Length == 0){
					System.Array.Resize (ref parameters, parameters.Length + 1);
					parameters[parameters.Length - 1] = "None";
					List<string> list= new List<string>(parameters);
					list.Swap(0,parameters.Length-1);
					parameters=list.ToArray();
				}
				
				for(int i=0;i< parameters.Length;i++){
					if(parameters[i] == param.Name){
						selectedIndex=i;
					}
				}
				GUI.color=(parameters[selectedIndex]=="None"?Color.red:Color.white);
				selectedIndex=EditorGUI.Popup(position,label.text,selectedIndex,parameters);
				GUI.color=Color.white;
				if(parameters[selectedIndex]!="None"){
					param.Name=parameters[selectedIndex];
				}
			}
		}

	}

	private NamedParameter CreateUserParameter(){
		NamedParameter param=(NamedParameter)ScriptableObject.CreateInstance(fieldInfo.FieldType);
		param.userVariable=true;
		AssetDatabase.AddObjectToAsset (param, controller);
		AssetDatabase.SaveAssets();
		return param;
	}
}
