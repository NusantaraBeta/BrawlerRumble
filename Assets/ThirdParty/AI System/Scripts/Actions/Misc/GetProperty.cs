using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

namespace AISystem.Actions{
	[Category("Misc")]
	[System.Serializable]
	public class GetProperty : BaseAction {
		#if UNITY_EDITOR
		public UnityEditor.MonoScript script;
		#endif
		[HideInInspector]
		public string scriptName;
		[HideInInspector]
		[StoreParameter(true,typeof(FloatParameter))]
		public string storeIntOrFloat; 
		[HideInInspector]
		[StoreParameter(true,typeof(BoolParameter))]
		public string storeBool; 
		[HideInInspector]
		[StoreParameter(true,typeof(StringParameter))]
		public string storeString; 
		[HideInInspector]
		public string property;

		public override void OnEnter ()
		{
			if (ownerDefault != null && ownerDefault.GetComponent (scriptName) != null) {
				MonoBehaviour behaviour = ownerDefault.GetComponent (scriptName) as MonoBehaviour;
				FieldInfo info = behaviour.GetType ()
					.GetFields (BindingFlags.Public | BindingFlags.Instance)
						.Where (x => x.Name == property).First ();
				
				if (info.FieldType == typeof(int)) {
					owner.SetFloat (storeIntOrFloat, (int)info.GetValue (behaviour));
				} else if (info.FieldType == typeof(float)) {
					owner.SetFloat (storeIntOrFloat, (float)info.GetValue (behaviour));
					Debug.Log (owner.GetFloat (storeIntOrFloat));
				} else if (info.FieldType == typeof(bool)) {
					owner.SetBool (storeBool, (bool)info.GetValue (behaviour));
				} else if (info.FieldType == typeof(string)) {
					
				}
			}
			Finish ();
		}
	}
}