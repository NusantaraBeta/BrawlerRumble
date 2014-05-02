using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(TagAttribute))]
public class TagDrawer : PropertyDrawer {
	string tag;
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
		position.x+=4;
		position.width -= 4;
		tag=EditorGUI.TagField(position,label.text,property.stringValue); 
		property.stringValue = tag;
	}
}
