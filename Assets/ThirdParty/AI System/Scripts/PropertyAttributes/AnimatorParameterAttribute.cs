using UnityEngine;
using System.Collections;

public class AnimatorParameterAttribute : PropertyAttribute {
	public AnimatorParameter type;

	public AnimatorParameterAttribute(AnimatorParameter type){
		this.type = type;
	}
}

public enum AnimatorParameter{
	State,
	Float,
	Bool,
	Int,
	Trigger
}