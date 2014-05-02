using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AISystem.Actions;

namespace AISystem.States{
	[System.Serializable]
	public class State : Node {
		public bool isDefaultState;
		public List<BaseTransition> transitions;
		public List<BaseAction> actions;
		[System.NonSerialized]
		public AIRuntimeController owner;

		private List<BaseAction> queueActions;
		private List<BaseAction> updateActions;

		public void Initialize(AIRuntimeController controller){
			this.owner = controller;
			queueActions = new List<BaseAction> ();
			updateActions = new List<BaseAction> ();

			for(int i=0;i< actions.Count;i++) {
				actions[i]=(BaseAction)ScriptableObject.Instantiate(actions[i]);
				actions[i].owner=owner;
				actions[i].OnAwake();
				if(actions[i].queue){
					queueActions.Add(actions[i]);
				}else{
					updateActions.Add(actions[i]);
				}
			}
			for(int k=0;k<transitions.Count;k++) {
				transitions[k]=(BaseTransition)ScriptableObject.Instantiate(transitions[k]);
				for(int i=0;i<transitions[k].conditions.Count;i++){
					transitions[k].conditions[i]=(BaseCondition)ScriptableObject.Instantiate(transitions[k].conditions[i]);
					transitions[k].conditions[i].owner=owner;
				}
				
				transitions[k].owner=owner;
			}
			this.DoAwake ();

		}

		public virtual void OnAwake(){}
		
		public virtual void OnEnter(){}
		
		public virtual void OnExit(){}
		
		public virtual void OnFixedUpdate(){}
		
		public virtual void OnUpdate(){}
		
		public virtual void OnLateUpdate(){}
		
		public virtual void OnAnimatorIK(int layerIndex){}
		
		public virtual void OnAnimatorMove(){}

		public State ValidateTransitions(){
			int count=transitions.Count;
			for(int i=0; i < count; i++) {
				State state = transitions[i].Validate();
				if(state != null){
					return state;
				}
			}
			return null;
		}

		private void DoAwake(){
			this.OnAwake ();
			for(int i=0;i<transitions.Count;i++){
				transitions[i].DoAwake();
			}
		}

		public void DoUpdate(){
			DoActions (UpdateType.OnUpdate);
			OnUpdate ();

		}

		public void DoFixedUpdate(){
			DoActions (UpdateType.OnFixedUpdate);
			OnFixedUpdate ();
		}

		private int queueIndex;
		private void DoActions(UpdateType updateType){
			for (int i=0; i< updateActions.Count; i++) {
				updateActions[i].OnEnter();
				switch(updateType){
				case UpdateType.OnUpdate:
					updateActions[i].OnUpdate();
					break;
				case UpdateType.OnLateUpdate:
					break;
				case UpdateType.OnFixedUpdate:
					updateActions[i].OnFixedUpdate();
					break;
				case UpdateType.OnAnimatorIK:
					break;
				case UpdateType.OnAnimatorMove:
					updateActions[i].OnAnimatorMove();
					break;
				}
				updateActions[i].OnExit();
			}


			if (queueActions.Count > 0) {
				if (queueActions [queueIndex].finished) {
					queueActions [queueIndex].OnExit();
					queueActions [queueIndex].Reset ();
					queueIndex++;
					if (queueIndex > (queueActions.Count-1)) {
						queueIndex = 0;
					}
					queueActions [queueIndex].OnEnter();
				}

				switch(updateType){
				case UpdateType.OnUpdate:
					queueActions [queueIndex].OnUpdate ();
					break;
				case UpdateType.OnLateUpdate:
					break;
				case UpdateType.OnFixedUpdate:
					queueActions [queueIndex].OnFixedUpdate ();
					break;
				case UpdateType.OnAnimatorIK:
					break;
				case UpdateType.OnAnimatorMove:
					queueActions [queueIndex].OnAnimatorMove ();
					break;
				}
			
			}
		}

		public void DoEnter(){
			OnEnter ();

			if (queueActions.Count > 0) {
				queueActions [queueIndex].OnEnter();
			}

			for(int i=0;i<transitions.Count;i++){
				transitions[i].DoEnter();
			}
		}

		public void DoExit(){
			OnExit ();
			if (queueActions.Count > 0) {
				queueActions [queueIndex].OnExit ();
				for (int i=0; i< queueActions.Count; i++) {
					queueActions [i].Reset ();
				}
				queueIndex = 0;
			}
		}

		public void DoAnimatorIK(int layerIndex){
			OnAnimatorIK (layerIndex);
			for (int i=0; i< updateActions.Count; i++) {
				updateActions[i].OnAnimatorIK(layerIndex);
			}
		}

		public void DoAnimatorMove(){
			OnAnimatorMove ();
			DoActions(UpdateType.OnAnimatorMove);
		}
	}
}