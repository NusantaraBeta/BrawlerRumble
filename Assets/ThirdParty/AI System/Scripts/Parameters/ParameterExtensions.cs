using UnityEngine;
using System.Collections;
using AISystem;

public static class ParameterExtensions {

	public static GameObject GetGameObject(this AIRuntimeController controller,string name){
		GameObjectParameter param = (GameObjectParameter)controller.GetParameter (name);
		return (param != null ? param.Value : null);
	}

	public static void SetGameObject(this AIRuntimeController controller,string name, GameObject go){
		GameObjectParameter param = (GameObjectParameter)controller.GetParameter (name);
		param.Value = go;
	}

	public static float GetFloat(this AIRuntimeController controller,string name){
		FloatParameter param = (FloatParameter)controller.GetParameter (name);
		return (param != null ? param.Value : 0);
	}

	public static void SetFloat(this AIRuntimeController controller,string name,float value){
		FloatParameter param = (FloatParameter)controller.GetParameter (name);
		param.Value = value;
	}

	public static string GetString(this AIRuntimeController controller,string name){
		StringParameter param = (StringParameter)controller.GetParameter (name);
		return (param != null ? param.Value : string.Empty);
	}
	
	public static void SetString(this AIRuntimeController controller,string name,string value){
		StringParameter param = (StringParameter)controller.GetParameter (name);
		param.Value = value;
	}


	public static void SetVector3(this AIRuntimeController controller,string name,Vector3 value){
		Vector3Parameter param = (Vector3Parameter)controller.GetParameter (name);
		param.Value = value;
	}

	public static Vector3 GetVector3(this AIRuntimeController controller,string name){
		NamedParameter param = controller.GetParameter (name);
		if (param is GameObjectParameter) {
			return ((GameObjectParameter)param).Value.transform.position;	
		}
		Vector3Parameter v = (Vector3Parameter)controller.GetParameter (name);
		return (v != null ? v.Value : Vector3.zero);
	}

	public static bool GetBool(this AIRuntimeController controller,string name){
		BoolParameter param = (BoolParameter)controller.GetParameter (name);
		return (param != null ? param.Value : false);
	}
	
	public static void SetBool(this AIRuntimeController controller,string name,bool value){
		BoolParameter param = (BoolParameter)controller.GetParameter (name);
		param.Value = value;
	}


	//->
	public static float GetValue(this AIRuntimeController controller,FloatParameter param){
		return string.IsNullOrEmpty (param.Name) ? param.Value : controller.GetFloat (param.Name);
	}

	public static bool GetValue(this AIRuntimeController controller,BoolParameter param){
		return string.IsNullOrEmpty (param.Name) ? param.Value : controller.GetBool (param.Name);
	}

	public static string GetValue(this AIRuntimeController controller,StringParameter param){
		return string.IsNullOrEmpty (param.Name) ? param.Value : controller.GetString (param.Name);
	}

	public static Vector3 GetValue(this AIRuntimeController controller,Vector3Parameter param){

		return string.IsNullOrEmpty (param.Name) ? param.Value : controller.GetVector3 (param.Name);
	}

	public static GameObject GetValue(this AIRuntimeController controller,GameObjectParameter param){
		return string.IsNullOrEmpty (param.Name) ? param.Value : controller.GetGameObject (param.Name);
	}
}
