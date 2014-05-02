using UnityEngine;
using System.Collections;

namespace AISystem.Actions.Animator{
	[Category("Animator")]
	[System.Serializable]
	public class SetIKPosition : BaseAction {
		public FloatParameter weight;
		public AvatarIKGoal goal;
		public Vector3Parameter position;
		
		private UnityEngine.Animator animator;
		
		public override void OnEnter (){
			animator = ownerDefault.GetComponent<UnityEngine.Animator> ();
		}
		
		public override void OnAnimatorIK (int layerIndex)
		{
			if (animator != null) {
				animator.SetIKPositionWeight (goal, owner.GetValue (weight));
				animator.SetIKPosition (goal, owner.GetValue (position));
			}
			Finish ();
		}
	}
}