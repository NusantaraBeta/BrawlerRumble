using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Reflection;
using System.Linq;

[CustomPropertyDrawer(typeof(ArrayElementAttribute))]
public class ArrayElementDrawer : PropertyDrawer {
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		position.x -= 10;
		Rect rect = new Rect (position);
		rect.width = 20;
		bool remove = false;
		if (GUI.Button (rect,ReorderableList.IconContent("Toolbar Minus","Remove point"),"label")) {
			remove=true;
		}
		position.x += 10;
		EditorGUI.PropertyField(position, property,GUIContent.none);
		if (remove) {
			SerializedProperty arrayProp=property.serializedObject.FindProperty(property.propertyPath.Split('.')[0]);
			arrayProp.DeleteArrayElementAtIndex(GetIndex(property));
		}
	}

	public int GetIndex(SerializedProperty prop)
	{
		return  Int32.Parse( System.Text.RegularExpressions.Regex.Match(prop.propertyPath, @"\d+").Value);
	}

}
