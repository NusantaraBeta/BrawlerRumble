using UnityEngine;
using System.Collections;

namespace AISystem{
	[System.Serializable]
	public class BaseCondition : ScriptableObject {
		private void OnEnable(){
			hideFlags = HideFlags.HideInHierarchy;
		}
		[System.NonSerialized]
		public AIRuntimeController owner;

		public virtual void OnAwake(){}

		public virtual void OnEnter(){}

		public virtual bool Validate(){
			return false;
		}
	}
}