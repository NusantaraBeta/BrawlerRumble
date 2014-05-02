using UnityEngine;
using System.Collections;

namespace AISystem.Actions{
	[System.Serializable]
	public class BaseAction:ScriptableObject  {
		[OwnerDefault]
		public string gameObject;

		public GameObject ownerDefault{
			get{
				return gameObject=="Owner" || string.IsNullOrEmpty(gameObject)?owner.gameObject:owner.GetGameObject(gameObject);
			}
		}

		[System.NonSerialized]
		public AIRuntimeController owner;

		public bool queue=false;
		[System.NonSerialized]
		public bool finished;
		
		private void OnEnable(){
			hideFlags = HideFlags.HideInHierarchy;
		}
		
		public void Finish(){
			this.finished = true;
		}

		public void Reset(){
			this.finished = false;
		}
		
		public virtual void OnAwake(){}
		
		public virtual void OnEnter(){}
		
		public virtual void OnExit(){}
		
		public virtual void OnFixedUpdate(){}
		
		public virtual void OnUpdate(){}

		public virtual void OnAnimatorIK(int layerIndex){}

		public virtual void OnAnimatorMove(){}
	}
}
