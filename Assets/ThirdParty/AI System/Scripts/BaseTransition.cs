using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AISystem.States;

namespace AISystem{
	[System.Serializable]
	public class BaseTransition : ScriptableObject {
		[System.NonSerialized]
		public AIRuntimeController owner;
		public State fromState;
		public State toState;
		public List<BaseCondition> conditions;
		
		private void OnEnable(){
			hideFlags = HideFlags.HideInHierarchy;
		}

		public void DoAwake(){
			int count = conditions.Count;
			for(int i=0;i< count;i++) {
				conditions[i].OnAwake();
			}
		}

		public void DoEnter(){
			int count = conditions.Count;
			for(int i=0;i< count;i++) {
				conditions[i].OnEnter();
			}
		}
		
		public State Validate(){
			int count = conditions.Count;
			for(int i=0;i< count;i++) {
				if(!conditions[i].Validate()){
					return null;
				}
			}
			return toState;
		}
	}
}