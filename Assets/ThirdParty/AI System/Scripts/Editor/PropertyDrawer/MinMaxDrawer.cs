using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(MinMaxAttribute))]
public class MinMaxDrawer : PropertyDrawer {

	MinMaxAttribute minMaxAttribute { get { return ((MinMaxAttribute)attribute); } }

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		position.x+=4;
		SerializedProperty minProperty = property.FindPropertyRelative ("min");
		SerializedProperty maxProperty = property.FindPropertyRelative ("max");
		float min = minProperty.floatValue;
		float max = maxProperty.floatValue;
		label.text="Min: "+ min.ToString("F1")+ " Max: "+max.ToString("F1") ;
		EditorGUI.MinMaxSlider(label, position,ref min,ref max,minMaxAttribute.minLimit, minMaxAttribute.maxLimit);
		if (minMaxAttribute.roundToInt) {
			min=Mathf.RoundToInt(min);
			max=Mathf.RoundToInt(max);
		}

		minProperty.floatValue = min;
		maxProperty.floatValue = max;
	}
}
